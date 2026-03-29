rmc-immune-to-ignition-examine = [color=cyan]{CAPITALIZE(SUBJECT($ent))} can't be {$direct ->
    [true]{""}
    *[false]{"indirectly "}
    }ignited![/color]
rmc-immune-to-fire-tile-damage-examine = [color=cyan]{CAPITALIZE(SUBJECT($ent))} takes no damage from tile fires![/color]
rmc-fire-armor-debuff-modifier-examine = [color=cyan]{CAPITALIZE(SUBJECT($ent))} has {POSS-ADJ($ent)} armor reduced {$percentage}% less when standing on green fire![/color]

rmc-molotov-craft-examine = [color=cyan]You can turn this into a molotov with a piece of paper![/color]
rmc-molotov-empty = The { $item } is empty...
rmc-molotov-low-fuel = There's not enough flammable liquid in the { $item }!

rmc-flammable-pat-user = You try to put out the fire on { THE($target) }!
rmc-flammable-pat-target = { THE($user) } tries to put out the fire on you!
rmc-flammable-pat-others = { THE($user) } tries to put out the fire on { THE($target) }!
