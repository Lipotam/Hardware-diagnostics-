using System.Collections.Generic;

namespace HardwareControl.Lab7
{
    public static class CellFactory
    {
        public static MemCell GetConstFalseCell()
        {
            return new MemCell(MemErrorTypes.SAF_0, new List<int>());
        }

        public static MemCell GetConstTrueCell()
        {
            return new MemCell(MemErrorTypes.SAF_1, new List<int>());
        }

        public static MemCell GetOkCell()
        {
            return new MemCell(MemErrorTypes.OK, new List<int>());
        }

        public static MemCell GetUnreachebleCell()
        {
            return new MemCell(MemErrorTypes.AF_cell_is_unreachable, new List<int>());
        }

        public static MemCell GetNoCellonAddressCell()
        {
            return new MemCell(MemErrorTypes.AF_no_cells_on_the_address, new List<int>());
        }

        public static List<MemCell> GetMultipleCellsOnAddressCell(int indexOfTheCell)
        {
            List<MemCell> cellsForTheError = new List<MemCell>
                {
                                new MemCell(MemErrorTypes.AF_multiple_cells_on_address, new List<int>
                                    {
                                                    indexOfTheCell + 1, indexOfTheCell + 2
                                    }),
                                GetOkCell(),
                                GetOkCell()
                };

            return cellsForTheError;
        }

        public static List<MemCell> GetMultipleAddressesOnCell(int indexOfTheCell)
        {
            List<MemCell> cellsForTheError = new List<MemCell>
                {
                               GetOkCell(),
                               new MemCell(MemErrorTypes.AF_multiple_addresses_on_the_Cell, new List<int>
                                    {
                                                    indexOfTheCell
                                    }),
                               new MemCell(MemErrorTypes.AF_multiple_addresses_on_the_Cell, new List<int>
                                    {
                                                    indexOfTheCell
                                    }),
                };

            return cellsForTheError;
        }

        public static List<MemCell> GetCFinAddressLessUpCell(int indexOfTheCell)
        {
            List<MemCell> cellsForTheError = new List<MemCell>
                {
                               new MemCell(MemErrorTypes.CFin_aggressor_addressLess_up, new List<int>
                                    {
                                                    indexOfTheCell+1
                                    }),
                               new MemCell(MemErrorTypes.CFin_victiom, new List<int>())
                };

            return cellsForTheError;
        }

        public static List<MemCell> GetCFinAddressLessDownCell(int indexOfTheCell)
        {
            List<MemCell> cellsForTheError = new List<MemCell>
                {
                               new MemCell(MemErrorTypes.CFin_aggressor_addressLess_down, new List<int>
                                    {
                                                    indexOfTheCell+1
                                    }),
                               new MemCell(MemErrorTypes.CFin_victiom, new List<int>())
                };

            return cellsForTheError;
        }

        public static List<MemCell> GetCFinAddressMoreUpCell(int indexOfTheCell)
        {
            List<MemCell> cellsForTheError = new List<MemCell>
                {
                               new MemCell(MemErrorTypes.CFin_victiom, new List<int>()),
                               new MemCell(MemErrorTypes.CFin_aggressor_addressMore_up, new List<int>
                                    {
                                                    indexOfTheCell
                                    })
                };

            return cellsForTheError;
        }

        public static List<MemCell> GetCFinAddressMoreDownCell(int indexOfTheCell)
        {
            List<MemCell> cellsForTheError = new List<MemCell>
                {
                               new MemCell(MemErrorTypes.CFin_victiom, new List<int>()),
                               new MemCell(MemErrorTypes.CFin_aggressor_addressMore_down, new List<int>
                                    {
                                                    indexOfTheCell
                                    })
                };

            return cellsForTheError;
        }

        public static List<MemCell> GetCFidAddressMoreDownSetTrueCell(int indexOfTheCell)
        {
            List<MemCell> cellsForTheError = new List<MemCell>
                {
                               new MemCell(MemErrorTypes.CFid_victiom, new List<int>()),
                               new MemCell(MemErrorTypes.CFid_aggressor_addressMore_down_set_true, new List<int>
                                    {
                                                    indexOfTheCell
                                    })
                };

            return cellsForTheError;
        }

        public static List<MemCell> GetCFidAddressMoreDownSetFalseCell(int indexOfTheCell)
        {
            List<MemCell> cellsForTheError = new List<MemCell>
                {
                               new MemCell(MemErrorTypes.CFid_victiom, new List<int>()),
                               new MemCell(MemErrorTypes.CFid_aggressor_addressMore_down_set_false, new List<int>
                                    {
                                                    indexOfTheCell
                                    })
                };

            return cellsForTheError;
        }

        public static List<MemCell> GetCFidAddressMoreUpSetFalseCell(int indexOfTheCell)
        {
            List<MemCell> cellsForTheError = new List<MemCell>
                {
                               new MemCell(MemErrorTypes.CFid_victiom, new List<int>()),
                               new MemCell(MemErrorTypes.CFid_aggressor_addressMore_up_set_false, new List<int>
                                    {
                                                    indexOfTheCell
                                    })
                };

            return cellsForTheError;
        }

        public static List<MemCell> GetCFidAddressMoreUpSetTrueCell(int indexOfTheCell)
        {
            List<MemCell> cellsForTheError = new List<MemCell>
                {
                               new MemCell(MemErrorTypes.CFid_victiom, new List<int>()),
                               new MemCell(MemErrorTypes.CFid_aggressor_addressMore_up_set_true, new List<int>
                                    {
                                                    indexOfTheCell
                                    })
                };

            return cellsForTheError;
        }

        public static List<MemCell> GetCFidAddressLessUpSetTrueCell(int indexOfTheCell)
        {
            List<MemCell> cellsForTheError = new List<MemCell>
                {
                               new MemCell(MemErrorTypes.CFid_aggressor_addressLess_up_set_true, new List<int>
                                    {
                                                    indexOfTheCell+1
                                    }),
                               new MemCell(MemErrorTypes.CFid_victiom, new List<int>())
                };

            return cellsForTheError;
        }

        public static List<MemCell> GetCFidAddressLessUpSetFalseCell(int indexOfTheCell)
        {
            List<MemCell> cellsForTheError = new List<MemCell>
                {
                               new MemCell(MemErrorTypes.CFid_aggressor_addressLess_up_set_false, new List<int>
                                    {
                                                    indexOfTheCell+1
                                    }),
                               new MemCell(MemErrorTypes.CFid_victiom, new List<int>())
                };

            return cellsForTheError;
        }

        public static List<MemCell> GetCFidAddressLessDownSetFalseCell(int indexOfTheCell)
        {
            List<MemCell> cellsForTheError = new List<MemCell>
                {
                               new MemCell(MemErrorTypes.CFid_aggressor_addressLess_down_set_false, new List<int>
                                    {
                                                    indexOfTheCell
                                    }),
                               new MemCell(MemErrorTypes.CFid_victiom, new List<int>())
                };

            return cellsForTheError;
        }

        public static List<MemCell> GetCFidAddressLessDownSetTrueCell(int indexOfTheCell)
        {
            List<MemCell> cellsForTheError = new List<MemCell>
                {
                               
                               new MemCell(MemErrorTypes.CFid_aggressor_addressLess_down_set_true, new List<int>
                                    {
                                                    indexOfTheCell
                                    }),
                               new MemCell(MemErrorTypes.CFid_victiom, new List<int>())
                };

            return cellsForTheError;
        }

        public static List<MemCell> GetPNPSFK3Cell(int indexOfTheCell)
        {
            List<MemCell> cellsForTheError = new List<MemCell>
                {
                               GetOkCell(),
                               new MemCell(MemErrorTypes.PNPSFK3, new List<int>()),
                               GetOkCell()
                };

            return cellsForTheError;
        }

        public static List<MemCell> GetANPSFK3Cell(int indexOfTheCell)
        {
            List<MemCell> cellsForTheError = new List<MemCell>
                {
                               new MemCell(MemErrorTypes.ANPSFK3, new List<int>{indexOfTheCell + 1}),
                               GetOkCell(),
                               new MemCell(MemErrorTypes.ANPSFK3, new List<int>{indexOfTheCell + 1}),
                };

            return cellsForTheError;
        }
    }
}
