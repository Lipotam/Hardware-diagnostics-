﻿using System;
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
        private List<string> cellStatesMessages;
        private List<string> cellPassStatesResult;

        public CellsMatrix(int x = 10, int y = 10)
        {
            cells = new List<MemCell>();

            xDimention = x;
            yDimention = y;

            this.cellStatesMessages = new List<string>();
            this.cellPassStatesResult = new List<string>();
            InitializeCells();
            for (int i = 0; i < cells.Count; i++)
            {
                this.cellStatesMessages.Add("");
                this.cellPassStatesResult.Add("");
            }

            ReportCellStates();
        }

        public List<string> GetResultMessages()
        {
            return this.cellStatesMessages;
        }

        public List<string> GetTestPassResultsMessages()
        {
            return this.cellPassStatesResult;
        }

        private void InitializeCells()
        {
            Random rand = new Random();
            while (cells.Count < xDimention * yDimention)
            {
                int randomValue = rand.Next(100);

                switch (randomValue)
                {
                    case 0:
                    case 1:
                        cells.Add(CellFactory.GetNoCellonAddressCell());
                        break;
                    case 2:
                    case 3:
                        cells.AddRange(CellFactory.GetMultipleCellsOnAddressCell(cells.Count));
                        break;
                    case 4:
                    case 5:
                        cells.AddRange(CellFactory.GetMultipleAddressesOnCell(cells.Count));
                        break;
                    case 6:
                    case 7:
                        cells.Add(CellFactory.GetConstFalseCell());
                        break;
                    case 8:
                    case 9:
                        cells.Add(CellFactory.GetConstTrueCell());
                        break;
                    case 10:
                    case 11:
                        cells.AddRange(CellFactory.GetCFinAddressLessUpCell(cells.Count));
                        break;
                    case 12:
                    case 13:
                        cells.AddRange(CellFactory.GetCFinAddressLessDownCell(cells.Count));
                        break;
                    case 14:
                    case 15:
                        cells.AddRange(CellFactory.GetCFinAddressMoreUpCell(cells.Count));
                        break;
                    case 16:
                    case 17:
                        cells.AddRange(CellFactory.GetCFinAddressMoreDownCell(cells.Count));
                        break;
                    case 18:
                    case 19:
                        cells.AddRange(CellFactory.GetCFidAddressLessUpSetFalseCell(cells.Count));
                        break;
                    case 20:
                    case 21:
                        cells.AddRange(CellFactory.GetCFidAddressLessDownSetFalseCell(cells.Count));
                        break;
                    case 22:
                    case 23:
                        cells.AddRange(CellFactory.GetCFidAddressMoreUpSetFalseCell(cells.Count));
                        break;
                    case 24:
                    case 25:
                        cells.AddRange(CellFactory.GetCFidAddressMoreDownSetFalseCell(cells.Count));
                        break;
                    case 26:
                    case 27:
                        cells.AddRange(CellFactory.GetCFidAddressLessUpSetTrueCell(cells.Count));
                        break;
                    case 28:
                    case 29:
                        cells.AddRange(CellFactory.GetCFidAddressLessDownSetTrueCell(cells.Count));
                        break;
                    case 30:
                    case 31:
                        cells.AddRange(CellFactory.GetCFidAddressMoreUpSetTrueCell(cells.Count));
                        break;
                    case 32:
                    case 33:
                        cells.AddRange(CellFactory.GetCFidAddressMoreDownSetTrueCell(cells.Count));
                        break;
                    case 34:
                    case 35:
                        cells.AddRange(CellFactory.GetPNPSFK3Cell(cells.Count));
                        break;
                    case 36:
                    case 37:
                        cells.AddRange(CellFactory.GetANPSFK3Cell(cells.Count));
                        break;
                    case 38:
                    case 39:
                        cells.Add(CellFactory.GetUnreachebleCell());
                        break;
                    default:
                        cells.Add(CellFactory.GetOkCell());
                        break;
                }
            }
        }

        private void ReportCellStates()
        {
            int index = 0;
            foreach (MemCell memCell in cells)
            {
                switch (memCell.ErrorType)
                {
                    case MemErrorTypes.OK:
                        this.cellStatesMessages[index] += "OK";
                        break;
                    case MemErrorTypes.AF_no_cells_on_the_address:
                        this.cellStatesMessages[index] += "no cells on the address";
                        break;
                    case MemErrorTypes.AF_cell_is_unreachable:
                        this.cellStatesMessages[index] += "the cell is unreachable";
                        break;
                    case MemErrorTypes.AF_multiple_cells_on_address:
                        this.cellStatesMessages[index] += "multiple_cells_on_address";
                        break;
                    case MemErrorTypes.AF_multiple_addresses_on_the_Cell:
                        this.cellStatesMessages[index] += "multiple_addresses_on_the_Cell";
                        break;
                    case MemErrorTypes.SAF_0:
                        this.cellStatesMessages[index] += "const false";
                        break;
                    case MemErrorTypes.SAF_1:
                        this.cellStatesMessages[index] += "const true";
                        break;
                    case MemErrorTypes.CFin_aggressor_addressLess_up:
                        this.cellStatesMessages[index] += "CFin_aggressor_addressLess_up";
                        break;
                    case MemErrorTypes.CFin_aggressor_addressLess_down:
                        this.cellStatesMessages[index] += "CFin_aggressor_addressLess_down";
                        break;
                    case MemErrorTypes.CFin_aggressor_addressMore_up:
                        this.cellStatesMessages[index] += "CFin_aggressor_addressMore_up";
                        break;
                    case MemErrorTypes.CFin_aggressor_addressMore_down:
                        this.cellStatesMessages[index] += "CFin_aggressor_addressMore_down";
                        break;
                    case MemErrorTypes.CFin_victiom:
                        this.cellStatesMessages[index] += "CFin_victiom";
                        break;
                    case MemErrorTypes.CFid_aggressor_addressLess_up_set_false:
                        this.cellStatesMessages[index] += "CFid_aggressor_addressLess_up_set_false";
                        break;
                    case MemErrorTypes.CFid_aggressor_addressLess_down_set_false:
                        this.cellStatesMessages[index] += "CFid_aggressor_addressLess_down_set_false";
                        break;
                    case MemErrorTypes.CFid_aggressor_addressMore_up_set_false:
                        this.cellStatesMessages[index] += "CFid_aggressor_addressMore_up_set_false";
                        break;
                    case MemErrorTypes.CFid_aggressor_addressMore_down_set_false:
                        this.cellStatesMessages[index] += "CFid_aggressor_addressMore_down_set_false";
                        break;
                    case MemErrorTypes.CFid_aggressor_addressLess_up_set_true:
                        this.cellStatesMessages[index] += "CFid_aggressor_addressLess_up_set_true";
                        break;
                    case MemErrorTypes.CFid_aggressor_addressLess_down_set_true:
                        this.cellStatesMessages[index] += "CFid_aggressor_addressLess_down_set_true";
                        break;
                    case MemErrorTypes.CFid_aggressor_addressMore_up_set_true:
                        this.cellStatesMessages[index] += "CFid_aggressor_addressMore_up_set_true";
                        break;
                    case MemErrorTypes.CFid_aggressor_addressMore_down_set_true:
                        this.cellStatesMessages[index] += "CFid_aggressor_addressMore_down_set_true";
                        break;
                    case MemErrorTypes.CFid_victiom:
                        this.cellStatesMessages[index] += "CFid_victiom";
                        break;
                    case MemErrorTypes.PNPSFK3:
                        this.cellStatesMessages[index] += "PNPSFK3";
                        break;
                    case MemErrorTypes.ANPSFK3:
                        this.cellStatesMessages[index] += "ANPSFK3";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                index++;
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
            if (index < 0 || index > cells.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            switch (cells[index].ErrorType)
            {
                case MemErrorTypes.OK:
                case MemErrorTypes.CFin_victiom:
                case MemErrorTypes.CFid_victiom:
                    cells[index].CellValue = inputValue;
                    break;
                case MemErrorTypes.AF_no_cells_on_the_address:
                case MemErrorTypes.AF_cell_is_unreachable:
                case MemErrorTypes.SAF_0:
                case MemErrorTypes.SAF_1:
                    break;
                case MemErrorTypes.AF_multiple_addresses_on_the_Cell:
                case MemErrorTypes.AF_multiple_cells_on_address:
                    cells[index].CellValue = inputValue;
                    foreach (var memCell in cells[index].CellAddresses)
                    {
                        SetLogic(memCell, inputValue);
                    }
                    break;
                case MemErrorTypes.CFin_aggressor_addressLess_up:
                case MemErrorTypes.CFin_aggressor_addressMore_up:
                    if (cells[index].CellValue == ElementsValues.False && inputValue == ElementsValues.True)
                    {
                        RevertValue(cells[index].CellAddresses.First());
                    }
                    cells[index].CellValue = inputValue;
                    break;
                case MemErrorTypes.CFin_aggressor_addressLess_down:
                case MemErrorTypes.CFin_aggressor_addressMore_down:
                    if (cells[index].CellValue == ElementsValues.True && inputValue == ElementsValues.False)
                    {
                        RevertValue(cells[index].CellAddresses.First());
                    }
                    cells[index].CellValue = inputValue;
                    break;
                case MemErrorTypes.CFid_aggressor_addressLess_up_set_false:
                case MemErrorTypes.CFid_aggressor_addressMore_up_set_false:
                case MemErrorTypes.CFid_aggressor_addressMore_down_set_false:
                case MemErrorTypes.CFid_aggressor_addressLess_down_set_false:
                    if ((cells[index].CellValue == ElementsValues.True && inputValue == ElementsValues.False) || (cells[index].CellValue == ElementsValues.False && inputValue == ElementsValues.True))
                    {
                        cells[cells[index].CellAddresses.First()].CellValue = ElementsValues.False;
                    }
                    cells[index].CellValue = inputValue;
                    break;
                case MemErrorTypes.CFid_aggressor_addressLess_up_set_true:
                case MemErrorTypes.CFid_aggressor_addressMore_up_set_true:
                case MemErrorTypes.CFid_aggressor_addressMore_down_set_true:
                case MemErrorTypes.CFid_aggressor_addressLess_down_set_true:
                    
                    if ((cells[index].CellValue == ElementsValues.True && inputValue == ElementsValues.False) || (cells[index].CellValue == ElementsValues.False && inputValue == ElementsValues.True))
                    {
                        cells[cells[index].CellAddresses.First()].CellValue = ElementsValues.True;
                    }
                    cells[index].CellValue = inputValue;
                    break;
                case MemErrorTypes.ANPSFK3:
                    cells[index].CellValue = inputValue;
                    if ((cells[index].CellValue == ElementsValues.True && inputValue == ElementsValues.False) || (cells[index].CellValue == ElementsValues.False && inputValue == ElementsValues.True))
                    {
                        RevertValue(cells[index].CellAddresses.First());
                    }
                    break;
                case MemErrorTypes.PNPSFK3:
                    if (cells[index - 1].CellValue == ElementsValues.True && cells[index + 1].CellValue == ElementsValues.True)
                    {
                        cells[index].CellValue = inputValue;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool IsEqualToFalse(int index)
        {
            if (cells[index].ErrorType == MemErrorTypes.AF_multiple_addresses_on_the_Cell)
            {
                index = cells[index].CellAddresses.First();
            }

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
            if (cells[index].ErrorType == MemErrorTypes.AF_multiple_addresses_on_the_Cell)
            {
                index = cells[index].CellAddresses.First();
            }

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
                    for (int i = cells.Count - 1; i > 0; i--)
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
            LogTestResult();
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
            LogTestResult();
        }


        public void Walking0To1Algoritm()
        {
            testPass = new bool[cells.Count()];

            for (int i = 0; i < yDimention - 1; i++)
            {
                for (int j = 0; j < xDimention - 1; j++)
                {
                    DoWalkingTest(i * xDimention + j);
                }
            }

            LogTestResult();
        }

        private void DoWalkingTest(int index)
        {
            int maxIndex = cells.Count - 1;

            SetFalse(index);

            if (index - xDimention - 1 > 0)
            {
                SetTrue(index - xDimention - 1);
            }
            if (index - xDimention > 0)
            {
                SetTrue(index - xDimention);
            }
            if (index - xDimention + 1 > 0)
            {
                SetTrue(index - xDimention + 1);
            }
            if (index - 1 > 0)
            {
                SetTrue(index - 1);
            }
            if (index + 1 < maxIndex)
            {
                SetTrue(index + 1);
            }
            if (index + xDimention - 1 < maxIndex)
            {
                SetTrue(index + xDimention - 1);
            }
            if (index + xDimention < maxIndex)
            {
                SetTrue(index + xDimention);
            }
            if (index + xDimention + 1 < maxIndex)
            {
                SetTrue(index + xDimention + 1);
            }


            bool result = IsEqualToFalse(index);

            if (index - xDimention - 1 > 0)
            {
                result &= IsEqualToTrue(index - xDimention - 1);
            }
            if (index - xDimention > 0)
            {
                result &= IsEqualToTrue(index - xDimention);
            }
            if (index - xDimention + 1 > 0)
            {
                result &= IsEqualToTrue(index - xDimention + 1);
            }
            if (index - 1 > 0)
            {
                result &= IsEqualToTrue(index - 1);
            }
            if (index + 1 < maxIndex)
            {
                result &= IsEqualToTrue(index + 1);
            }
            if (index + xDimention - 1 < maxIndex)
            {
                result &= IsEqualToTrue(index + xDimention - 1);
            }
            if (index + xDimention < maxIndex)
            {
                result &= IsEqualToTrue(index + xDimention - 1);
            }
            if (index + xDimention + 1 < maxIndex)
            {
                result &= IsEqualToTrue(index + xDimention + 1);
            }

            testPass[index] = result;
        }

        private void LogTestResult()
        {
            cellPassStatesResult = new List<string>();
            for (int i = 0; i < cells.Count; i++)
            {
                this.cellPassStatesResult.Add(testPass[i] ? "Passed " : "Not Passed ");
            }
        }
    }
}
