namespace GateOps.Domain.GateOperations;

public enum GateDirection
{
    Inbound,  // container entering the terminal (e.g. import discharge, empty return)
    Outbound  // container leaving the terminal (e.g. export pickup, empty pickup)
}
