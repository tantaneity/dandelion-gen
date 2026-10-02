# dandelion-gen

A dandelion grown in Unity out of arithmetic. No models, no textures, no sculpting. The flower, the clock and the stalk are meshes built in C# from a few dozen numbers, and the whole life of the plant runs off a single parameter from 0 to 1.

![clock](media/clock.png)

## Phyllotaxis

Everything on a dandelion is placed by one angle. 137.507764 degrees, the golden angle, the turn that never lines up with itself no matter how many times you repeat it. Two placements fall out of it.

```
sphere   seeds on the clock, one per index, spiralling pole to pole
disc     florets in the yellow head, radius growing as sqrt(index)
```

That's the whole layout code. Nobody places a seed by hand, nobody tweaks a gap, and the packing comes out even because an irrational turn can't help but be even.

![bloom](media/bloom.png)

## One parameter, one life

Bud, opening, flower, closing, greying, the clock swelling out, seeds leaving, bare receptacle. All of it is a function of `life`.

```
0.00  tight bud, bracts closed, florets hidden
0.33  fully open, 265 ligulate florets on a flattened dome
0.60  shut again
0.64  the head turns grey
0.80  the clock is full, 125 seeds on a Fibonacci lattice
1.00  the cap is bare and pitted, the involucre dry
```

![lifecycle](media/lifecycle.png)

Each stage is a smoothstep between two thresholds, so the plant never jumps. Ask for `life = 0.47` and you get a flower caught halfway through closing, which is a frame nobody authored.

## Seeds leave without being remembered

Each seed draws its own departure threshold from a hash of its index. Past that threshold its position is a plain function of how far the blow has progressed: drift along the wind, a little rise, a slow tumble. Nothing accumulates, nothing is stored.

So frame 600 renders without frames 1 through 599 ever existing. Scrub anywhere, render out of order, split the job across machines, the dandelion doesn't care.

![blow](media/blow.png)

And the receptacle pulls the same trick. A pit is drawn exactly where a seed used to be, so the cap bares itself as the seeds go, with no bookkeeping about which ones left. Seeds and pits both read the surface from one function, which means the cap can swell and flatten late in life and nothing floats off it.

![receptacle](media/receptacle.png)

## The look

Flat fills, one light, a hard shadow band, and ink along the silhouette. The shader is the same one driving the other generators in this family, with stroke width always a fraction of screen height, so a hair stays a hair whether the camera is across the meadow or an inch away.

Built with Unity 6000.3.5f2 and URP.

## Licence

MIT.
