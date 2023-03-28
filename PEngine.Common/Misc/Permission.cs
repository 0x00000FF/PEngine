namespace PEngine.Common.Misc;

public enum Permission
{
    NoPerm = 0,
    
    OtherX = 0x001,
    OtherW = 0x002,
    OtherR = 0x004,
    
    RoleX = 0x008,
    RoleW = 0x010,
    RoleR = 0x020,
    
    OwnerX = 0x040,
    OwnerW = 0x080,
    OwnerR = 0x100
}