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

        public CellsMatrix(int x = 10, int y = 10)
        {
            cells = new List<MemCell>();
            xDimention = x;
            yDimention = y;
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

        public void SetFalse(int index)
        {
            SetLogic(index, ElementsValues.False);
        }

        public void SetTrue(int index)
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

        public bool IsEqualToFalse(int index)
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
        public bool IsEqualToTrue(int index)
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

    }
}
