local k = 
{
    name="Y",
    age=23,
    Bye = function()
        print("123")
    end,
}

local mt = {
    __index = k
}

local t = {sex = "boy"}
setmetatable(t,mt)

print(t.sex)
print(t.name)
print(t.enjoy)
t.Bye()
t.bye()