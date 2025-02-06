Welcome to the the restaurant! #speaker:npc #trigger: next
-> main

=== main ===
What would you like to order?
    +[Fish Pasty]
        I would like a Fish Pasty #speaker:player #trigger: next
        -> chosen("Fish Pasty", 10)
    +[Fish Pie]
        I would like a Fish Pie #speaker:player #trigger: next
        -> chosen("Fish Pie", 20)

=== chosen(dish, cookTime) ===
Ok i will serve {dish} in {cookTime} minutes  #speaker:npc #trigger: next
Would you like something else? #trigger: next
    +[Yes]
        -> main
    +[No]
        Alright, please wait for your order #trigger: next
        ->END


