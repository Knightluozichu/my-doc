local t1 = {}
local t2 = {}

local mt = {
    __div = function(arg1, arg2)
        print("div")
    end

}

setmetatable(t1, mt)

local b = t1 / t2