using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// each vertex has info and edges(graph)
/// </summary>
public class Vertex
{
    public State Info { get; set; }
    public List<State> Edges { get; set; }

    public Vertex(State info)
    {
        this.Info = info;
        //Edges = edges;
        this.Edges = new List<State>();
    }


}
/// <summary>
/// data structure of the game map(42 territories(each territory is a vertex))
/// </summary>
public class Graph
{

    // An adjacency list to hold graph data
    public List<Vertex> map { get; set; }

    //constractor
    public Graph()
    {
        this.map = new List<Vertex>();
        createMap();
    }

    /// <summary>
    /// add a vertex the map
    /// </summary>
    /// <param name="v">the vertex to add</param>
    protected void addVertexNoCheck(Vertex v)
    {
        this.map.Add(v);

    }
    /// <summary>
    /// add Edges to both vertex(tie between the territories in the map)
    /// </summary>
    /// <param name="vertex1">the first vertex</param>
    /// <param name="vertex2">the second vertex</param>
    protected void addAnEdgeForMap(Vertex vertex1, Vertex vertex2)
    {
        // assume all vertices are valid and already exist.

        // Add an vertex2 to vertex1 edges.
        vertex1.Edges.Add(vertex2.Info);

        // Add an vertex1 to vertex2 edges.
        vertex2.Edges.Add(vertex1.Info);

    }

    /// <summary>
    /// add a vertex the map if the vertex bot exist already
    /// </summary>
    /// <param name="v">the vertex to add</param>
    /// <returns>success or not</returns>
    private Boolean addVertex(Vertex v)
    {

        // If the vertex already exists, do nothing.
        if (this.map.Find(e => e.Info == v.Info) != null)
        {
            return true;
        }

        this.map.Add(v);
        return true;
    }

    /// <summary>
    /// add Edges to both vertex(tie between the territories in the map) if not connected already
    /// </summary>
    /// <param name="vertex1">One of the vertexes</param>
    /// <param name="vertex2">the second vertex</param>
    /// <returns></returns>
    private Boolean addAnEdge(Vertex vertex1, Vertex vertex2)
    {
        // assume all vertices are valid and already exist.

        // Add an vertex2 to vertex1 edges.
        this.map.Find(e => e.Info == vertex1.Info).Edges.Add(vertex2.Info);

        // Add an vertex1 to vertex2 edges.
        this.map.Find(e => e.Info == vertex2.Info).Edges.Add(vertex1.Info);
        return true;

    }

    /// <summary>
    /// create the map for the game (changed for diffrent worlds )
    /// </summary>
    private void createMap()
    {
        //create the territories
        Vertex v1 = new Vertex(new State("Alaska", 1));
        Vertex v2 = new Vertex(new State("NorthWest Territory", 2));
        Vertex v3 = new Vertex(new State("Greenland", 3));
        Vertex v4 = new Vertex(new State("Alberta", 4));
        Vertex v5 = new Vertex(new State("Ontario", 5));
        Vertex v6 = new Vertex(new State("Quebec", 6));
        Vertex v7 = new Vertex(new State("Westren United States", 7));
        Vertex v8 = new Vertex(new State("Eastern United States", 8));
        Vertex v9 = new Vertex(new State("Central America", 9));
        Vertex v10 = new Vertex(new State("Iceland", 10));
        Vertex v11 = new Vertex(new State("Scandanavia", 11));
        Vertex v12 = new Vertex(new State("Ukraine", 12));
        Vertex v13 = new Vertex(new State("Great Britian", 13));
        Vertex v14 = new Vertex(new State("Northern Europe", 14));
        Vertex v15 = new Vertex(new State("Western Europe", 15));
        Vertex v16 = new Vertex(new State("Southern Europe", 16));
        Vertex v17 = new Vertex(new State("Ural", 17));
        Vertex v18 = new Vertex(new State("Siberia", 18));
        Vertex v19 = new Vertex(new State("Yakutsk", 19));
        Vertex v20 = new Vertex(new State("Kamchatka", 20));
        Vertex v21 = new Vertex(new State("Afganistan", 21));
        Vertex v22 = new Vertex(new State("China", 22));
        Vertex v23 = new Vertex(new State("Mongolia", 23));
        Vertex v24 = new Vertex(new State("Irkutsk", 24));
        Vertex v25 = new Vertex(new State("Middle East", 25));
        Vertex v26 = new Vertex(new State("India", 26));
        Vertex v27 = new Vertex(new State("Siam", 27));
        Vertex v28 = new Vertex(new State("Japan", 28));
        Vertex v29 = new Vertex(new State("Venezuela", 29));
        Vertex v30 = new Vertex(new State("Peru", 30));
        Vertex v31 = new Vertex(new State("Brazil", 31));
        Vertex v32 = new Vertex(new State("Argentina", 32));
        Vertex v33 = new Vertex(new State("North Africa", 33));
        Vertex v34 = new Vertex(new State("Egypt", 34));
        Vertex v35 = new Vertex(new State("East Africa", 35));
        Vertex v36 = new Vertex(new State("Congo", 36));
        Vertex v37 = new Vertex(new State("Madagascar", 37));
        Vertex v38 = new Vertex(new State("South Africa", 38));
        Vertex v39 = new Vertex(new State("Indonesia", 39));
        Vertex v40 = new Vertex(new State("New Guinea", 40));
        Vertex v41 = new Vertex(new State("Western Australia", 41));
        Vertex v42 = new Vertex(new State("Eastern Australia", 42));
        //add the ties(the connectios between the territories)
        addAnEdgeForMap(v1, v2);
        addAnEdgeForMap(v1, v4);
        addAnEdgeForMap(v1, v20);
        addAnEdgeForMap(v2, v3);
        addAnEdgeForMap(v2, v4);
        addAnEdgeForMap(v3, v5);
        addAnEdgeForMap(v3, v6);
        addAnEdgeForMap(v3, v10);
        addAnEdgeForMap(v4, v5);
        addAnEdgeForMap(v4, v7);
        addAnEdgeForMap(v5, v6);
        addAnEdgeForMap(v5, v8);
        addAnEdgeForMap(v5, v7);
        addAnEdgeForMap(v6, v8);
        addAnEdgeForMap(v7, v8);
        addAnEdgeForMap(v7, v9);
        addAnEdgeForMap(v8, v9);
        addAnEdgeForMap(v9, v29);
        addAnEdgeForMap(v10, v11);
        addAnEdgeForMap(v10, v13);
        addAnEdgeForMap(v11, v12);
        addAnEdgeForMap(v11, v13);
        addAnEdgeForMap(v11, v14);
        addAnEdgeForMap(v12, v14);
        addAnEdgeForMap(v12, v16);
        addAnEdgeForMap(v12, v17);
        addAnEdgeForMap(v12, v21);
        addAnEdgeForMap(v12, v25);
        addAnEdgeForMap(v13, v14);
        addAnEdgeForMap(v13, v15);
        addAnEdgeForMap(v14, v15);
        addAnEdgeForMap(v14, v16);
        addAnEdgeForMap(v15, v16);
        addAnEdgeForMap(v15, v33);
        addAnEdgeForMap(v16, v33);
        addAnEdgeForMap(v16, v34);
        addAnEdgeForMap(v16, v25);
        addAnEdgeForMap(v17, v18);
        addAnEdgeForMap(v17, v21);
        addAnEdgeForMap(v17, v22);
        addAnEdgeForMap(v18, v19);
        addAnEdgeForMap(v18, v22);
        addAnEdgeForMap(v18, v23);
        addAnEdgeForMap(v18, v24);
        addAnEdgeForMap(v19, v20);
        addAnEdgeForMap(v19, v24);
        addAnEdgeForMap(v20, v23);
        addAnEdgeForMap(v20, v24);
        addAnEdgeForMap(v20, v28);
        addAnEdgeForMap(v21, v22);
        addAnEdgeForMap(v21, v25);
        addAnEdgeForMap(v21, v26);
        addAnEdgeForMap(v22, v23);
        addAnEdgeForMap(v22, v26);
        addAnEdgeForMap(v22, v27);
        addAnEdgeForMap(v23, v24);
        addAnEdgeForMap(v23, v28);
        addAnEdgeForMap(v25, v26);
        addAnEdgeForMap(v25, v34);
        addAnEdgeForMap(v26, v27);
        addAnEdgeForMap(v27, v39);
        addAnEdgeForMap(v29, v30);
        addAnEdgeForMap(v29, v31);
        addAnEdgeForMap(v30, v31);
        addAnEdgeForMap(v30, v32);
        addAnEdgeForMap(v31, v32);
        addAnEdgeForMap(v31, v33);
        addAnEdgeForMap(v33, v34);
        addAnEdgeForMap(v33, v35);
        addAnEdgeForMap(v33, v36);
        addAnEdgeForMap(v34, v35);
        addAnEdgeForMap(v35, v36);
        addAnEdgeForMap(v35, v37);
        addAnEdgeForMap(v35, v38);
        addAnEdgeForMap(v36, v38);
        addAnEdgeForMap(v37, v38);
        addAnEdgeForMap(v39, v40);
        addAnEdgeForMap(v39, v41);
        addAnEdgeForMap(v40, v41);
        addAnEdgeForMap(v40, v42);
        addAnEdgeForMap(v41, v42);

        //add to the list of territories
        addVertexNoCheck(v1);
        addVertexNoCheck(v2);
        addVertexNoCheck(v3);
        addVertexNoCheck(v4);
        addVertexNoCheck(v5);
        addVertexNoCheck(v6);
        addVertexNoCheck(v7);
        addVertexNoCheck(v8);
        addVertexNoCheck(v9);
        addVertexNoCheck(v10);
        addVertexNoCheck(v11);
        addVertexNoCheck(v12);
        addVertexNoCheck(v13);
        addVertexNoCheck(v14);
        addVertexNoCheck(v15);
        addVertexNoCheck(v16);
        addVertexNoCheck(v17);
        addVertexNoCheck(v18);
        addVertexNoCheck(v19);
        addVertexNoCheck(v20);
        addVertexNoCheck(v21);
        addVertexNoCheck(v22);
        addVertexNoCheck(v23);
        addVertexNoCheck(v24);
        addVertexNoCheck(v25);
        addVertexNoCheck(v26);
        addVertexNoCheck(v27);
        addVertexNoCheck(v28);
        addVertexNoCheck(v29);
        addVertexNoCheck(v30);
        addVertexNoCheck(v31);
        addVertexNoCheck(v32);
        addVertexNoCheck(v33);
        addVertexNoCheck(v34);
        addVertexNoCheck(v35);
        addVertexNoCheck(v36);
        addVertexNoCheck(v37);
        addVertexNoCheck(v38);
        addVertexNoCheck(v39);
        addVertexNoCheck(v40);
        addVertexNoCheck(v41);
        addVertexNoCheck(v42);

    }

    /// <summary>
    /// check if the vertex in the index of "index_of_vertex2" is in the list of edges(neighbor) of the vertex in index of "index_of_vertex1"
    /// </summary>
    /// <param name="index_of_vertex1"></param>
    /// <param name="index_of_vertex2"></param>
    /// <returns>true if vertex2 is neighbor </returns>
    public bool vertex_is_neighbor(int index_of_vertex1, int index_of_vertex2)
    {
        return Global.map.map[index_of_vertex1].Edges.Contains(Global.map.map[index_of_vertex2].Info);

    }
    /// <summary>
    /// get how many territories the player rules
    /// </summary>
    /// <param name="player">wich player to check</param>
    /// <returns>how many territories the player own</returns>
    public int how_many_forces(Global.Control player)
    {
        int counter = 0;
        foreach (Vertex ter in Global.map.map)
        {
            if (ter.Info.Ruled_by == player)
                counter++;
        }
        return counter;
    }
    /// <summary>
    /// get how many territories the player rules(and in how many he has more than one soldier)
    /// </summary>
    /// <param name="player">wich player to check</param>
    /// <returns>list of int, in index[0] how many territories the player own, in in index[1] how many territories with more than one soldiers the player own</returns>
    public List<int> number_of_forces(Global.Control player)
    {
        List<int> list = new List<int>();
        list.Add(0);//num of countntries
        list.Add(0);//countries where at least 2 soldiers
        foreach (Vertex ter in Global.map.map)
        {
            if (ter.Info.Ruled_by == player)
            {
                list[0]++;
                if (ter.Info.Num_of_forces > 1)
                {
                    list[1]++;
                }
            }

        }
        return list;
    }
    /// <summary>
    /// check if the given player rule the continet
    /// </summary>
    /// <param name="cont">which continet to check</param>
    /// <param name="player">which player to check</param>
    /// <returns>true if he owns it</returns>
    public bool has_continent(List<int> cont, Global.Control player)
    {
        foreach (int index_of_ter in cont)
        {
            if (Global.map.map[index_of_ter].Info.Ruled_by != player)
                return false;
        }

        return true;
    }
    /// <summary>
    /// create a list the represent the continet
    /// </summary>
    /// <param name="start">which index to start from</param>
    /// <param name="end">which index to stop</param>
    /// <returns>list of indexes "start" to "end"</returns>
    public List<int> create_list(int start, int end)
    {
        List<int> continent = new List<int>();
        for (int i = start; i < end; i++)
        {
            continent.Add(i);
        }

        return continent;
    }
    /// <summary>
    /// create Asia
    /// </summary>
    /// <returns>a list of the indexes</returns>
    public List<int> create_list_of_Asia()
    {
        return create_list(16, 28);
    }
    /// <summary>
    /// create North America
    /// </summary>
    /// <returns>a list of the indexes</returns>
    public List<int> create_list_of_NorthAmerica()
    {
        return create_list(0, 9);
    }
    /// <summary>
    /// create South America
    /// </summary>
    /// <returns>a list of the indexes</returns>
    public List<int> create_list_of_SouthAmerica()
    {
        return create_list(28, 32);
    }
    /// <summary>
    /// create Eurpe
    /// </summary>
    /// <returns>a list of the indexes</returns>
    public List<int> create_list_of_Europe()
    {
        return create_list(9, 16);
    }
    /// <summary>
    /// create Africa
    /// </summary>
    /// <returns>a list of the indexes</returns>
    public List<int> create_list_of_Africa()
    {
        return create_list(32, 38);
    }
    /// <summary>
    /// create Australia
    /// </summary>
    /// <returns>a list of the indexes</returns>
    public List<int> create_list_of_Australia()
    {
        return create_list(38, 42);
    }

    /// <summary>
    /// copy the map
    /// </summary>
    /// <param name="mapToCopy">get the map to copy from</param>
    public void copyBoard(Graph mapToCopy)
    {
        for (int i = 0; i < mapToCopy.map.Count; i++)
        {
            this.map[i].Info.Ruled_by = mapToCopy.map[i].Info.Ruled_by;
            this.map[i].Info.Num_of_forces = mapToCopy.map[i].Info.Num_of_forces;
        }
    }


}
