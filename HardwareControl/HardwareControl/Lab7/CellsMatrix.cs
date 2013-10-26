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
                    foreach (var memCell in cells[index].CellAddresses)
                    {
                        SetLogic(memCell, inputValue);
                    }
                    break;
                case MemErrorTypes.CFin_aggressor_addressLess_up:
                case MemErrorTypes.CFin_aggressor_addressMore_up:
                    if (cells[index].CellValue == ElementsValues.False && inputValue == ElementsValues.True)
                    {
                        if (cells[cells[index].CellAddresses.First()].CellValue == ElementsValues.False)
                        {
                            cells[cells[index].CellAddresses.First()].CellValue = ElementsValues.True;
                        }
                        if (cells[cells[index].CellAddresses.First()].CellValue == ElementsValues.True)
                        {
                            cells[cells[index].CellAddresses.First()].CellValue = ElementsValues.False;
                        }
                    }
                    cells[index].CellValue = inputValue;
                    break;
                case MemErrorTypes.CFin_aggressor_addressLess_down:
                case MemErrorTypes.CFin_aggressor_addressMore_down:
                    if (cells[index].CellValue == ElementsValues.True && inputValue == ElementsValues.False)
                    {
                        if (cells[cells[index].CellAddresses.First()].CellValue == ElementsValues.False)
                        {
                            cells[cells[index].CellAddresses.First()].CellValue = ElementsValues.True;
                        }
                        if (cells[cells[index].CellAddresses.First()].CellValue == ElementsValues.True)
                        {
                            cells[cells[index].CellAddresses.First()].CellValue = ElementsValues.False;
                        }
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
                    break;
                case MemErrorTypes.CFid_aggressor_addressLess_up_set_true:
                case MemErrorTypes.CFid_aggressor_addressMore_up_set_true:
                case MemErrorTypes.CFid_aggressor_addressMore_down_set_true:
                case MemErrorTypes.CFid_aggressor_addressLess_down_set_true:
                    if ((cells[index].CellValue == ElementsValues.True && inputValue == ElementsValues.False) || (cells[index].CellValue == ElementsValues.False && inputValue == ElementsValues.True))
                    {
                        cells[cells[index].CellAddresses.First()].CellValue = ElementsValues.True;
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
    }
}
