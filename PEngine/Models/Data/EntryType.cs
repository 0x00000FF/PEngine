namespace PEngine.Models.Data;

public enum EntryType
{
    Post = 1,
    Tag = 1 << 1,
    Comment = 1 << 2,
    File = 1 << 3,
    Directory = 1 << 4
}