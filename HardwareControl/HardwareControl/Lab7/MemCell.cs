using System.Collections.Generic;
using HardwareControl.Elements;

namespace HardwareControl.Lab7
{
    class MemCell
    {
        private MemErrorTypes errorType;
        private ElementsValues cellValue;
        private readonly List<int> addresses;

        public MemErrorTypes ErrorType
        {
            get
            {
                return errorType;
            }
            set
            {
                errorType = value;
            }
        }


        public ElementsValues CellValue
        {
            get
            {
                switch (errorType)
                {
                    case MemErrorTypes.AF_cell_is_unreachable:
                    case MemErrorTypes.AF_no_cells_on_the_address:
                    case MemErrorTypes.AF_multiple_cells_on_address:
                        return ElementsValues.Undefined;
                    case MemErrorTypes.SAF_0:
                        return ElementsValues.False;
                    case MemErrorTypes.SAF_1:
                        return ElementsValues.True;
                    default:
                        return cellValue;
                }
            }

            set
            {
                switch (errorType)
                {
                    case MemErrorTypes.AF_cell_is_unreachable:
                    case MemErrorTypes.AF_no_cells_on_the_address:
                    case MemErrorTypes.SAF_0:
                    case MemErrorTypes.SAF_1:
                        return;
                    default:
                        cellValue = value;
                        break;
                }
            }
        }

        public List<int> CellAddresses
        {
            get
            {
                return addresses;
            }
        }

        public MemCell(MemErrorTypes cellErrorType, List<int> cellAddresses)
        {
            errorType = cellErrorType;
            addresses = cellAddresses;
        }
    }
}
