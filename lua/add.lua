local t1 = {age = 23}
local t2 = { age = 15}

local mt = {
    __add = function(arg1, arg2)
        return arg1.age + arg2.age
    end
}

setmetatable(t1, mt)
setmetatable(t2, mt)
print(t1 + t2)