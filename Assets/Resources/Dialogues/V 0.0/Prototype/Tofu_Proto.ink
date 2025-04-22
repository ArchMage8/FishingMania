EXTERNAL EnableShop()

Looking to go fishing? #speaker: Tofu
Got all you need?
-> main

=== main ===
    +[Fishing?]
        ->prompt1
    +[I need some bait]
        ->prompt2



=== prompt1 ===
Yes fishing...
You can head to the pier on the beach there.
Plenty of mackarel to go around...
->END

=== prompt2 ===
Sure, got some right here...
~ EnableShop()
-> DONE
