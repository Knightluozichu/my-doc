local t1 = {key=10}
local t2 = {key = -5.6}
local mt = {
    __mul = function(arg1, arg2)
        print(arg1.key * arg2.key)
    end
}


setmetatable(t2, mt)

local b = t2 * t1