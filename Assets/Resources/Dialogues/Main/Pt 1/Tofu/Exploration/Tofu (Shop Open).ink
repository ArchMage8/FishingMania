EXTERNAL EnableShop()

Looking to go fishing? #speaker: Tofu #trigger: next
Got all you need? #speaker: Tofu #trigger: next
->main

=== main ===
    +[Fishing?]
        ->section1
    +[I need some bait.]
        ->section5
    +[See ya!]
        ->section4

=== section1 ===
Yes, fishing!
There's a spot right there on the pier.
Plenty of mackerel to go around.

    +[How do I fish?]
    ->section2
    
    +[See ya!]
    ->section4

=== section2 ===
Well, first you need bait.
You need to use different types of baits for different water bodies.

Then there is your hook!
The better your hook is, the better your chances are at catching a better fish.

Once you have chosen your equipment, just find a fishing spot and cast your line.

There are several of them here and on the other islands.
->section3

=== section3 ===
    +[Can I buy some bait from you?]
    ->section5
    
    +[Ah, ok then.]
    ->section6

=== section4 ===
Goodbye!
->END

=== section5 ===
Sure, got some right here. #speaker: Tofu #trigger: next
~ EnableShop()
->END

=== section6 ===
Come back to me if you want to buy some bait.
->END