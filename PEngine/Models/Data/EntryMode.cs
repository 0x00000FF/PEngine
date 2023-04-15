namespace PEngine.Models.Data;

[Flags]
public enum EntryMode
{
    Post = 1 << 9,
    Comment = 1 << 10,
    File = 1 << 11,
    Directory = 1 << 12,
    
    OwnerRead = 1 << 8,
    OwnerWrite = 1 << 7,
    OwnerExecute = 1 << 6,
    
    GroupRead = 1 << 5,
    GroupWrite = 1 << 4,
    GroupExecute = 1 << 3,
    
    OtherRead = 1 << 2,
    OtherWrite = 1 << 1,
    OtherExecute = 1,
}