# Lookup
The Lookup feature allows you to conduct a 'reverse search' on the world. By selecting a location, and finding all entities at that position.

## Note: Spotlight installation is advised
It is recommended (particularly when viewing the main Appalachia worldspace), that Spotlight is installed to make the best use of lookup. As for all large spaces the features work in tandem, ensuring you can see the necessary details for lookup to label against. You can continue to use lookup without spotlight installed, which will work fine for most *interior* spaces.

[Read about spotlight here.](Spotlight.md#installing-spotlight)

## Using Lookup
To activate lookup, simply right-click the preview window and select 'Lookup Here'. A circle will be drawn indicating the searched range, and all detected items are labelled with lines pointing to them.<br/>

The types of items returned are influenced by the 'Filter by Category' selections on the main Appalachia window.<br/>
Leaving bounding boxes ('Plot Settings' > 'Draw Bounds') on is recommended, as it best clearly identifies each labelled item.

## Lookup Settings
Under 'Map' > 'Lookup' you can control the behavior of Lookup:

- Range: The range of the circular area to search, from 64 -> 1,024 game units.
- Arrangement: Affects how the labels are positioned around the circle.
- Draw Volumes: Typically entities considered "volumes" are invisible. They are labelled by lookup but their extents are not clear. Toggle this to control if volumes are rendered in addition to being labelled.
- Fuzzy Search: Off: Only entities whose origin lies within the range are drawn. On: Entities whose bounding box may (given any arbitrary rotation) intersect the range are drawn. This is useful for finding identifying larger objects by only selecting parts of them, but may find more results than you intended.

## Next guide
[Recipes](Recipes.md)
