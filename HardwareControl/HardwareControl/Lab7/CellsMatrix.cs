using System;
using System.Collections.Generic;
using System.Linq;
using HardwareControl.Elements;

namespace HardwareControl.Lab7
{
    class CellsMatrix
    {
        private List<MemCell> cells;
        private int xDimention, yDimention;
        private bool[] testPass;
        private List<string> resultMessages;

        public CellsMatrix(int x = 10, int y = 10)
        {
            cells = new List<MemCell>();

            xDimention = x;
            yDimention = y;

            resultMessages = new List<string>();

            for (int i = 0; i < cells.Count; i++)
            {
                resultMessages.Add("");
            }
        }

        public int Width
        {
            get
            {
                return xDimention;
            }
        }

        public int Height
        {
            get
            {
                return yDimention;
            }
        }

        private void SetFalse(int index)
        {
            SetLogic(index, ElementsValues.False);
        }

        private void SetTrue(int index)
        {
            SetLogic(index, ElementsValues.True);
        }

        private void SetLogic(int index, ElementsValues inputValue)
        {
            switch (cells[index].ErrorType)
            {
                case MemErrorTypes.OK:
                case MemErrorTypes.CFin_victiom:
                case MemErrorTypes.CFid_victiom:
                case MemErrorTypes.AF_multiple_addresses_on_the_Cell:
                    cells[index].CellValue = inputValue;
                    break;
                case MemErrorTypes.AF_no_cells_on_the_address:
                case MemErrorTypes.AF_cell_is_unreachable:
                case MemErrorTypes.SAF_0:
                case MemErrorTypes.SAF_1:
                    break;
                case MemErrorTypes.AF_multiple_cells_on_address:
                    cells[index].CellValue = inputValue;
                    foreach (var memCell in cells[index].CellAddresses)
                    {
                        SetLogic(memCell, inputValue);
                    }
                    break;
                case MemErrorTypes.CFin_aggressor_addressLess_up:
                case MemErrorTypes.CFin_aggressor_addressMore_up:
                    cells[index].CellValue = inputValue;
                    if (cells[index].CellValue == ElementsValues.False && inputValue == ElementsValues.True)
                    {
                        RevertValue(cells[index].CellAddresses.First());
                    }
                    break;
                case MemErrorTypes.CFin_aggressor_addressLess_down:
                case MemErrorTypes.CFin_aggressor_addressMore_down:
                    cells[index].CellValue = inputValue;
                    if (cells[index].CellValue == ElementsValues.True && inputValue == ElementsValues.False)
                    {
                        RevertValue(cells[index].CellAddresses.First());
                    }
                    break;
                case MemErrorTypes.CFid_aggressor_addressLess_up_set_false:
                case MemErrorTypes.CFid_aggressor_addressMore_up_set_false:
                case MemErrorTypes.CFid_aggressor_addressMore_down_set_false:
                case MemErrorTypes.CFid_aggressor_addressLess_down_set_false:
                    cells[index].CellValue = inputValue;
                    if ((cells[index].CellValue == ElementsValues.True && inputValue == ElementsValues.False) || (cells[index].CellValue == ElementsValues.False && inputValue == ElementsValues.True))
                    {
                        cells[cells[index].CellAddresses.First()].CellValue = ElementsValues.False;
                    }
                    break;
                case MemErrorTypes.CFid_aggressor_addressLess_up_set_true:
                case MemErrorTypes.CFid_aggressor_addressMore_up_set_true:
                case MemErrorTypes.CFid_aggressor_addressMore_down_set_true:
                case MemErrorTypes.CFid_aggressor_addressLess_down_set_true:
                    cells[index].CellValue = inputValue;
                    if ((cells[index].CellValue == ElementsValues.True && inputValue == ElementsValues.False) || (cells[index].CellValue == ElementsValues.False && inputValue == ElementsValues.True))
                    {
                        cells[cells[index].CellAddresses.First()].CellValue = ElementsValues.True;
                    }
                    break;
                case MemErrorTypes.ANPSFK3:
                    cells[index].CellValue = inputValue;
                    if ((cells[index].CellValue == ElementsValues.True && inputValue == ElementsValues.False) || (cells[index].CellValue == ElementsValues.False && inputValue == ElementsValues.True))
                    {
                        RevertValue(cells[index].CellAddresses.First());
                    }
                    break;
                case MemErrorTypes.PNPSFK3:
                    cells[index].CellValue = inputValue;
                    if ((cells[index].CellValue == ElementsValues.True && inputValue == ElementsValues.False) || (cells[index].CellValue == ElementsValues.False && inputValue == ElementsValues.True))
                    {
                        if (cells[cells[index].CellAddresses.First() + 1].CellValue == ElementsValues.True && cells[cells[index].CellAddresses.First() - 1].CellValue == ElementsValues.True)
                        {
                            RevertValue(cells[index].CellAddresses.First());
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool IsEqualToFalse(int index)
        {
            if (cells[index].CellValue == ElementsValues.False)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsEqualToTrue(int index)
        {
            if (cells[index].CellValue == ElementsValues.True)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void RevertValue(int index)
        {
            if (cells[index].CellValue == ElementsValues.False)
            {
                cells[index].CellValue = ElementsValues.True;
            }
            if (cells[index].CellValue == ElementsValues.True)
            {
                cells[index].CellValue = ElementsValues.False;
            }
        }

        private void RunMarshTest(List<MarchTestQuery> marchTestQueries)
        {
            testPass = new bool[cells.Count()];
            for (int i = 0; i < cells.Count; i++)
            {
                testPass[i] = true;
            }

            foreach (var testQuery in marchTestQueries)
            {
                if (testQuery.AddressIncrement)
                {
                    for (int i = 0; i < cells.Count; i++)
                    {
                        DoQuery(i, testQuery.Functions);
                    }
                }
                else
                {
                    for (int i = cells.Count; i > 0; i++)
                    {
                        DoQuery(i, testQuery.Functions);
                    }
                }
            }
        }

        private void DoQuery(int index, List<MarshTestFunctions> testFunctions)
        {
            foreach (var marshTestFunction in testFunctions)
            {
                switch (marshTestFunction)
                {
                    case MarshTestFunctions.ReadFalse:
                        if (testPass[index])
                        {
                            testPass[index] = IsEqualToFalse(index);
                        }
                        break;
                    case MarshTestFunctions.ReadTrue:
                        if (testPass[index])
                        {
                            testPass[index] = IsEqualToTrue(index);
                        }
                        break;
                    case MarshTestFunctions.WriteFalse:
                        SetFalse(index);
                        break;
                    case MarshTestFunctions.WriteTrue:
                        SetTrue(index);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }


        public void AlgoritmB()
        {
            List<MarchTestQuery> testQueries = new List<MarchTestQuery>
                {
                        new MarchTestQuery
                            {
                                    AddressIncrement = true,
                                    Functions =
                                            new List<MarshTestFunctions>
                                                {
                                                        MarshTestFunctions.WriteFalse
                                                }
                            },
                        new MarchTestQuery
                            {
                                    AddressIncrement = true,
                                    Functions =
                                            new List<MarshTestFunctions>
                                                {
                                                        MarshTestFunctions.ReadFalse,
                                                        MarshTestFunctions.WriteTrue,
                                                        MarshTestFunctions.WriteFalse,
                                                        MarshTestFunctions.WriteTrue
                                                }
                            },
                        new MarchTestQuery
                            {
                                    AddressIncrement = true,
                                    Functions =
                                            new List<MarshTestFunctions>
                                                {
                                                        MarshTestFunctions.ReadTrue,
                                                        MarshTestFunctions.WriteFalse,
                                                        MarshTestFunctions.ReadFalse,
                                                        MarshTestFunctions.WriteTrue
                                                }
                            },
                        new MarchTestQuery
                            {
                                    AddressIncrement = false,
                                    Functions =
                                            new List<MarshTestFunctions>
                                                {
                                                        MarshTestFunctions.ReadTrue,
                                                        MarshTestFunctions.WriteFalse,
                                                        MarshTestFunctions.WriteTrue,
                                                        MarshTestFunctions.WriteFalse
                                                }
                            },
                        new MarchTestQuery
                            {
                                    AddressIncrement = false,
                                    Functions =
                                            new List<MarshTestFunctions>
                                                {
                                                        MarshTestFunctions.ReadFalse,
                                                        MarshTestFunctions.WriteTrue,
                                                        MarshTestFunctions.ReadTrue,
                                                        MarshTestFunctions.WriteFalse
                                                }
                            }
                };

            RunMarshTest(testQueries);

            for (int i = 0; i < cells.Count; i++)
            {
                resultMessages[i] += testPass[i] ? "Passed " : "Not Passed ";
            }
        }

        public void MarshPSAlgoritm()
        {
            List<MarchTestQuery> testQueries = new List<MarchTestQuery>
                {
                        new MarchTestQuery
                            {
                                    AddressIncrement = true,
                                    Functions =
                                            new List<MarshTestFunctions>
                                                {
                                                        MarshTestFunctions.WriteFalse
                                                }
                            },
                        new MarchTestQuery
                            {
                                    AddressIncrement = true,
                                    Functions =
                                            new List<MarshTestFunctions>
                                                {
                                                        MarshTestFunctions.ReadFalse,
                                                        MarshTestFunctions.WriteTrue,
                                                        MarshTestFunctions.ReadTrue,
                                                        MarshTestFunctions.WriteFalse,
                                                        MarshTestFunctions.ReadFalse,
                                                        MarshTestFunctions.WriteTrue
                                                }
                            },
                        new MarchTestQuery
                            {
                                    AddressIncrement = true,
                                    Functions =
                                            new List<MarshTestFunctions>
                                                {
                                                        MarshTestFunctions.ReadTrue,
                                                        MarshTestFunctions.WriteFalse,
                                                        MarshTestFunctions.ReadFalse,
                                                        MarshTestFunctions.WriteTrue,
                                                        MarshTestFunctions.ReadTrue
                                                }
                            },
                        new MarchTestQuery
                            {
                                    AddressIncrement = true,
                                    Functions =
                                            new List<MarshTestFunctions>
                                                {
                                                        MarshTestFunctions.ReadTrue,
                                                        MarshTestFunctions.WriteFalse,
                                                        MarshTestFunctions.ReadFalse,
                                                        MarshTestFunctions.WriteTrue,
                                                        MarshTestFunctions.ReadTrue,
                                                        MarshTestFunctions.WriteFalse
                                                }
                            },
                        new MarchTestQuery
                            {
                                    AddressIncrement = false,
                                    Functions =
                                            new List<MarshTestFunctions>
                                                {
                                                        MarshTestFunctions.ReadFalse,
                                                        MarshTestFunctions.WriteTrue,
                                                        MarshTestFunctions.ReadTrue,
                                                        MarshTestFunctions.WriteFalse,
                                                        MarshTestFunctions.ReadFalse
                                                }
                            }
                };

            RunMarshTest(testQueries);

            for (int i = 0; i < cells.Count; i++)
            {
                resultMessages[i] += testPass[i] ? "Passed " : "Not Passed ";
            }
        }
    }
}
