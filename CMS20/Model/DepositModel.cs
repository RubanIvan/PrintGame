using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CMS20.Model
{
    //получаемая колекция
    public class DepositFileArray
    {
        [JsonProperty("Files")]
        public DepositFile[] Files;
    }

    //описание одного файла
    public class DepositFile
    {
        [JsonProperty("download_cnt")]
        public long download_cnt;       //download_cnt=0

        [JsonProperty("download_url")]
        public string download_url;     //download_url=http://dfiles.ru/files/usp50xnu6

        [JsonProperty("dt_added")]
        public DateTime dt_added;       //dt_added=2016-02-28 19:57:53

        [JsonProperty("dt_expires")]
        public DateTime dt_expires;     //dt_expires=2016-05-28 19:57:53

        [JsonProperty("file_password")]
        public string file_password;    //  

        [JsonProperty("filename_source")]
        public string filename_source;  //filename_source=Dungeon Lords1.zip

        [JsonProperty("id")]
        public long id;                 //id=166145650

        [JsonProperty("id_str")]
        public string id_str;           //id_str=usp50xnu6

        [JsonProperty("size")]
        public long size;               //size=261597700
    }
}
