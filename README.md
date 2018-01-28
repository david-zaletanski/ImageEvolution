# ImageEvolution

**Updated README:** 1/28/2018

An experiment in evolution and fitness algorithms. The idea is that you load a picture and the application, by simulating evolution, will try to recreate the picture. It starts by randomly generating a number of circles of varying size and shape. In each generation, there is a chance for circles to "mutate", by changing size or shape. If the circle's mutation makes the newly produced image closer to the source image, it stays. Rinse and repeat thousands of times, and eventually you will have a replication of the source image that is made up from of circles. If memory serves, I believe the runtime for each generation was taking a long time, so I have also made it multithreaded.