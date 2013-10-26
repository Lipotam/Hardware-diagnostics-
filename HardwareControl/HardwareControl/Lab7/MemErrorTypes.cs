namespace HardwareControl.Lab7
{
    public enum MemErrorTypes
    {
        OK,
        AF_no_cells_on_the_address,
        AF_cell_is_unreachable,
        AF_multiple_cells_on_address,
        AF_multiple_addresses_on_the_Cell,
        SAF_0,
        SAF_1,

        CFin_aggressor_addressLess_up,
        CFin_aggressor_addressLess_down,
        CFin_aggressor_addressMore_up,
        CFin_aggressor_addressMore_down,
        CFin_victiom,

        CFid_aggressor_addressLess_up_set_false,
        CFid_aggressor_addressLess_down_set_false,
        CFid_aggressor_addressMore_up_set_false,
        CFid_aggressor_addressMore_down_set_false,
        CFid_aggressor_addressLess_up_set_true,
        CFid_aggressor_addressLess_down_set_true,
        CFid_aggressor_addressMore_up_set_true,
        CFid_aggressor_addressMore_down_set_true,
        CFid_victiom
    };
}
