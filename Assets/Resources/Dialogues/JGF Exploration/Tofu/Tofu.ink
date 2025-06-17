EXTERNAL EnableShop()

Hmm?#trigger: next
Looking to go fishing? #speaker: Tofu #trigger: next
Got all you need? #speaker: Tofu #trigger: next
->main

=== main ===
    +[Fishing?]
        ->section1
    +[Need?]
        ->section2
    +[I need some bait]
        ->section3

=== section1 ===
Yes fishing! #speaker: Tofu #trigger: next
You can head to the pier on the beach there. #speaker: Tofu #trigger: next
Plenty of mackarel to go around... #speaker: Tofu #trigger: next
->END

=== section2 ===
You can't just throw a rope into the water and expect something... #speaker: Tofu #trigger: next
You need a hook and some bait! #speaker: Tofu #trigger: next
->END

=== section3 ===
Sure, got some right here... #speaker: Tofu #trigger: next
~ EnableShop()
->END