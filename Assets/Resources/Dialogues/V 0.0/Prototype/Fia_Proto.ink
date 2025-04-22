EXTERNAL EnablePlayerShop()

Hey you look like a cook. #speaker: Fia
Got any dishes to sell?
I've got some issues in the back...
->main

=== main ===
    +[Issues?]
    ->prompt1
    +[Sure! Wanna see?]
    ->prompt2
    +[Umm, not right now]
    ->prompt3


=== prompt1 ===
This is a restaurant.
But my stove sort of...
Caught fire...
So I need to get dishes some other way...
->main


=== prompt2 ===
Ah thanks!
~ EnablePlayerShop()
-> DONE

=== prompt3 ===
Alright then...
-> DONE