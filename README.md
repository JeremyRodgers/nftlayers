# NFTLayers

## Name
NFT Layers

## Description
A little project done with an artist friend to generate a large amount of images for an NFT marketplace (when that was a thing for 15 minutes).  It takes a set of layers each of which has n image variations, the combination of which creates a unique image. The image manipulation is done with ImageMagick and the code is multi-threaded. I experimented with a few implementations of the multi-threaded routine using Parallel.ForEach and PLINQ. On a 20-thread Intel machine running Linux I was able to generate all ~7500 permutations in about 20 minutes.
