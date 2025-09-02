# Mappalachia Plotting functions

## Plotting Modes
Mappalachia has four plotting modes - these are Standard mode (on by default), Heatmap mode, Topography mode, and Cluster mode. You can toggle these modes by navigating to Plot Settings > Plot Mode.<br/>

## Standard Mode
In Standard mode, selected items will each be plotted using icons customizable by shape and color.<br/>
The default colors used can be controlled in 'Plot Settings' > 'Plot Styles' > 'Default Standard Palette'. You may also change the default icon size in the Plot Styles window.<br/>

If you wish, you can also influence the default icons by changing the files in the folder `img\icon\` of your installation.<br/>

Finally, you can edit the icon properties of individual plots by clicking the icon image in the 'Items selected to Plot' table.

## Topographic Mode
Topographic mode is similar to Icon mode, except it uses varying colors to visualize the altitude of plotted items. An altitude key is drawn on the right-hand-side of the map image. The colors used can be changed in 'Plot Settings' > 'Plot Styles' > 'Heatmap & Topographic palette'.<br/>
You may wish to select different plot icon shapes for items you need to distinguish, as the unique colors in standard mode are not used here.

## Heatmap Mode
When in Heatmap mode, Mappalachia will represent the density of items with an intensity of color on the map.<br/>
Heatmap mode is more appropriate for large collections of items such as flora - where it is more valuable to know not the exact positions of items, but rather where they have the highest density, and therefore where you should go to find the most.<br/>

The colors used can be changed in 'Plot Settings' > 'Plot Styles' > 'Heatmap & Topographic palette'.<br/>
The range and intensity of the heatmap can be controlled via 'Plot Settings' > 'Heatmap Settings'.

## Cluster mode
Cluster mode groups nearby points into bounded areas or 'clusters' and labels them with the number of entities inside the cluster.<br/>
The maximum size and minimum weight of clusters can be controlled under 'Plot Settings' > 'Cluster Settings'.<br/>
Here you may also choose if clusters are formed of all plotted items together, or if different legend groups form independent clusters.<br/>

*Tip: Right clicking the cluster radius text on this window will allow you set the range equivalent to the size of a nuke zone. This can be useful for finding flux (See [Flux and other derived results](Derived_results.md))*<br/>

The value drawn against the cluster represents the total 'weighted chance' of the cluster.

## Next guide
[Scrap, NPCs, and Flux](Derived_results.md)

