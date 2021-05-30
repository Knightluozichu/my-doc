local t1 = {}
local t2 = {}

local mt = {
    __sub = function(agr1, arg2)
        print("agr1 + agr2")
    end
}

setmetatable(t1, mt)

local b = t1 - t2
