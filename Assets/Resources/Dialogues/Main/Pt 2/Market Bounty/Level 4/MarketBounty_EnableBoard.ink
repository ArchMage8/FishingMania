EXTERNAL EnableShop()

Hey! welcome back!
->main

=== main ===
    +[I wanna check out the board!]
    ->section1
    
    +[I have some questions...]
    ->section2
    
    +[Nevermind.]
    ->section3
    
=== section1 ===
Sure! take a look!
~ EnableShop()
->END

=== section2 ===
What would you like to know?

    +[How does the board work again?]
    ->section2A
    +[Who makes the special orders?]
    ->section2B
    
=== section2A ===
Well, I put up the orders I have on the board, and if you can fulfill them, I give you a bonus.
Which is 3 times the market value of the fish...
Oh, and I put up new orders everyday!
->END

=== section2B ===
A lot of different people, from restaurants to just normal townsfolk.

Why don't they just buy fish from the store?

Well, not all fish types are available on demand there. If someone urgently needs something in particular, they place a special order.
->END

=== section3 ===
Ok then...
->END