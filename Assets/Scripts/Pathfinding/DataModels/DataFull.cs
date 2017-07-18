using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public class DataFull  {

    private string _fullName;

    public string FullName
    {
        get { return _fullName; }
        set { _fullName = value; }
    }



    List<DataZone> ListOfDestinations;
    public DataFull(string argMainWorldAnchorName) { _fullName = argMainWorldAnchorName; ListOfDestinations = new List<DataZone>(); }

    public List<DataZone> GetZones() { return this.ListOfDestinations; }
    public void AddToZones(DataZone argdataZone) { ListOfDestinations.Add(argdataZone);  }

}
