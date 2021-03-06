# ImageEvolution

**Updated README:** 1/28/2018

An experiment in natural selection, evolution and fitness algorithms. The overall idea is to load a picture and hit Run. In the first generation, it generates a number of circles with random size, shape, position, and color. Consider those circles are the initial group of "parents". In each subsequent generation, the parents create child circles, which are much like themselves although there is a chance that a random mutation may change one of these properties in the "child" circle. If that mutated child circle in the randomly generated image (made out of circles) look just a bit closer to the actual image the user initially chooses, it will stick around (only the fittest child circles survive). Rinse and repeat the process for a long time (thousands of generations) and you are simulating evolution.

I forget where I saw it, but the idea for demonstrating evolution with image comparisons like this was not mine, I simply recreated the application from a video I saw. It makes the concepts of evolution feel a lot more down to earth as you can visualize the picture improving in real time. By playing with various parameters like the mutation rate, you can see what impact this has on the overall development in real time.

This is really one of my favorite projects. I love biology and computers and this seems like a perfect intersection of concepts from both. The images that the computer comes up with will end up being very unique, like an artist's rendition, and very good given enough time. My last addition was making it multithreaded as the calculations are computation heavy, although I expect on modern systems a few hours of running will produce some impressive results.

## Experiment Log

| Attempt | Image | Mut. Rate | # Children | Avg Gen Dur | # Gen Completed | Duration | # Comments |
| :-----: | ----- | :-------: | :--------: | :---------: | :-------------: | :------: | :--------- |
| 1 | Darwin | 0.05 | 150 | 4.5s | 4622 | 6 hrs | Circles too small and spread apart, need to speed up.
| 2 | Darwin | 0.08 | 300 | 


## Notes

