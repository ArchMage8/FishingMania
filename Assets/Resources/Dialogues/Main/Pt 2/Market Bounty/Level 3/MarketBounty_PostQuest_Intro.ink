Ah, you're back! #speaker: Aoi #trigger: next
->main

=== main ===
Hey, I got a business proposition for you!
Remember the board I told you about?
    +[Yeah?]
    ->section1
    
    +[Not really.]
    ->clarify
    

=== clarify ===
It's the board I place all my special orders on... 

Ah, ok! #speaker: Iris #trigger: clarify
->section1

=== section1 === 
So here's my offer, everyday I'll put up some items I need for my orders. #speaker: Aoi #trigger: next
And if you deliver them, I'll give you 3 times their normal price, just like before.
How about it?
    +[Sure I'll take a look.]
    ->section2
    
    +[I'll think about it.]
    ->section2


=== section2 ===
Well the board's right here, you can come up to me should you want to make some extra coin!

    +[Thanks!]
    ->Exit

=== Exit ===
Don't mention it.
->END
