# metadataEnmapa
Pequeña aplicacion que obtiene las coordenadas desde la metadata de las fotos y las plasma en un mapa de google maps.

C#

This project started like some sort of challange, a friend of mine who is an expert in Autocad, Civil Design and 3DMax needed to extract the coordenates hidden in the metadata of some photos taken with a drone.  I told him it can be done with C# without really know if it could be done.

This was my first noneducational project done with C#.

For this i used two libraries i found in internet and i recomend, one ExifLib that has the code that extract the metadata from the photos. The other library was GMap.NET, this library allow me to represent the aforementioned coordinates in a map of Google Maps, it in theory works with other providers too.

This program take all the jpg files in a folder the user choose, search among them those with metadata/coordinates, show those files and their corresponding coordinates in a multiline richtextbox, also shows the representation of those points in the map.

Since i'm not an expert in C# (as you probably will notice due to the slopiness of the code) we can conclude that those libraries are relativelly easy to use.

Hope someone find this code usefull.
