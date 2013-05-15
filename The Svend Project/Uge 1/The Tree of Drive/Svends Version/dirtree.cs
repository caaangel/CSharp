using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

public class Program {

    public static void Main() {
        WalkDirectory(Directory.GetCurrentDirectory(), new ParentFolders());
    }

    static void WalkDirectory(string dir, ParentFolders parents) {
        var dirInfo = new DirectoryInfo(dir);
        if (dirInfo.Attributes.HasFlag(FileAttributes.Hidden)) return;

        Console.WriteLine(parents.ToString() + Path.GetFileName(dir));
        
        var subdirs = Directory.GetDirectories(dir);
        for (var i = 0; i < subdirs.Length; i++) {
            parents.Add(i < subdirs.Length - 1);
            WalkDirectory(subdirs[i], parents);
            parents.RemoveAt(parents.Count - 1);
        }
    }
}

class ParentFolders : List<bool> {
    public override string ToString() {
        var sb = new StringBuilder();
        var i = 0;
        foreach(var parent in this) {
            sb.Append(i++ < this.Count - 1 ?
                      parent ? "|   " : "    " :
                      parent ? "+---" : "\\---");
        }
        return sb.ToString();
    }
}