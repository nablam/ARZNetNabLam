// @Author Jeffrey M. Paquette ©2016

using System;
using System.IO;

public class WaveDemoData : WaveData
{
    public WaveDemoData(string fileName) : base(fileName) { }
    
    public float spawnFrequency = 7.0f;

    override public void WriteData(BinaryWriter writer)
    {
        writer.Write(spawnFrequency);
    }

    override public void ReadData(BinaryReader reader)
    {
        spawnFrequency = reader.ReadSingle();
    }
}
