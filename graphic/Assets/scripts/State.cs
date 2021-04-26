using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// the info of each vertex in the graph
/// </summary>
public class State
{

    int num_of_forces;
    string name_of_territor;
    Global.Control ruled_by;
    int index;

    /// <summary>
    /// create the info with initial values
    /// </summary>
    /// <param name="name_of_territor">the name of territory</param>
    /// <param name="index">the index of the territory in the list</param>
    public State(string name_of_territor, int index)
    {
        num_of_forces = 0;
        this.name_of_territor = name_of_territor;
        this.ruled_by = Global.Control.Free;
        this.index = index;
    }
    public int Num_of_forces { get => num_of_forces; set => num_of_forces = value; }
    public Global.Control Ruled_by { get => ruled_by; set => ruled_by = value; }
    public string Name_of_territor { get => name_of_territor; }
    public int Index { get => index; set => index = value; }
}

