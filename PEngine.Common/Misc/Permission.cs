namespace PEngine.Common.Misc;

public enum Permission
{
    NoPerm = 0,
    
    OtherX = 1,
    OtherW = 2,
    OtherR = 4,
    
    RoleX = 8,
    RoleW = 16,
    RoleR = 32,
    
    OwnerX = 64,
    OwnerW = 128,
    OwnerR = 256
}