
//DE50OperatingState


namespace CashAccSysDeviceManager
{
    public enum DE50OperatingState
    {
        Waiting,
        Counting,
        Counting_start_request,
        Abnormal_device,
        Being_reset,
        Being_store,
        Being_restoration,
        Being_exchange_the_cassette,
        Storing_start_request,
        Being_recover_from_Storing_error,
        Escrow_open_request,
        Escrow_close_request,
        Escrow_open,
        Escrow_close,
        Initialize_start_request,
        Begin_initialize,
        Being_set,
        Local_operation,
        Storing_error,
        Waiting_for_an_envelope_to_set,
    }
}
