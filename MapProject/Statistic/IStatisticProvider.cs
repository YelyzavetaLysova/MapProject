﻿using MapProject.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject.Statistic
{
    public interface IStatisticProvider
    {
        void SaveDataSet(DataSet set, Map map);

        DataSet GetDataSet(string dataSetKey, Map map);

        void RemoveDataSet(string dataSetKey, Map map);

        List<string> GetDataSets(Map map);

        void AddAttachment(string dataSetKey, string regionId, Map map, string pathToFile);

        //void GetAttachmentPath(string dataSetKey, string regionId, Map map, string pathToFile);
    }
}
