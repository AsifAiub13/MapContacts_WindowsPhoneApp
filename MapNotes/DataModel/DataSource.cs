using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage;
using System.Runtime.Serialization.Json;

namespace MapNotes.DataModel
{
    public class MapNote
    {
        public string Title { get; set; }
        public string Note { get; set; }
        public DateTime Created { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }



    }



    public class DataSource
    {
        private ObservableCollection<MapNote> _mapNotes;
        const string fileName = "mapnotes.json";

        public DataSource()
        {
            _mapNotes = new ObservableCollection<MapNote>();

        }

        public async Task<ObservableCollection<MapNote>> GetMapNotes()
        {
            await ensureDataLoaded();
            return _mapNotes;
        }

        private async Task ensureDataLoaded()
        {
            if (_mapNotes.Count == 0)
                await getMapNotDataAsync();
            return;


        }
        private async Task getMapNotDataAsync()
        {
            if (_mapNotes.Count != 0)
                return;
            var jsonSerializer = new DataContractJsonSerialozer(typeof(ObservableCollection<MapNote>));

            try
            {
                using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(fileName))
                {
                    _mapNotes = (ObservableCollection<MapNote>)jsonSerializer.ReadObject(stream);

                }
            }
            catch
            {

                _mapNotes = new ObservableCollection<MapNote>();

            }

        }
        public async void AddMapnote(MapNote mapNote)
        {
            _mapNotes.Add(mapNote);
            await saveMapNoteDataAsync();
        }

        public async void DeleteMapnote(MapNote mapNote)
        {
            _mapNotes.Remove(mapNote);
            await saveMapNoteDataAsync();
        }

        private async Task saveMapNoteDataAsync()
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<MapNote>));
            using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(fileName,
                CreationCollisionOption.ReplaceExisting))
            {
                jsonSerializer.WriteObject(stream, _mapNotes);


            }
        }
    }
}
