# VISION

The first idea was a galaxy-Management type game where we could be able to create procedural planet based on a webcam frame with color segmentation and delete those on demands using a trgger mooves the webcam was suppose to recognise (A snap ! Like thanos)
Thrilled but impossible whitin time limits and design isssues occured the more we thought about it 

So it became a tiny game where one can create a planet to his liking using colors inputs from the webcam , those colors will be projected onto the sphere and discover planet pattern like Earth

## Amelioration
- Generate biomes according to a specific colors (shades of blues creates oceans and rivers , shades of greens vegetation , white for clouds/gaz .. etc )
- Let people choose the name of a pattern they found , store them in a server for next players

## Issues 
- Computing is slow , should moove those computation into a computeshader
- Color segmentation : it is good to separate parts from frames but in my case i needed to answer the question ' When is a color consired to be blue or green or white ? What about shades ?' I ended up using Hue value in HSV colors 
- Planets are not perfect sphers generating unwanted blur effects at poles


### Dependencies 
- EmguCV 3.3
- Light Weight Render Pipeline
