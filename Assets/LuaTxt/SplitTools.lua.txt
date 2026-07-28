-- ============================================================
-- 字符串分割工具
-- 为 Lua 的 string 类型扩展一个 split 方法，按指定分隔符分割字符串
-- ============================================================

--- 将字符串按分隔符切割成若干子串，返回一个 table（数组）
--- @param input     string  待分割的原始字符串
--- @param delimiter string  分隔符
--- @return table|boolean    分割后的子串数组；若分隔符为空则返回 false
function string.split(input, delimiter)
    -- 确保输入参数为字符串类型（兼容数字等类型）
    input = tostring(input)
    delimiter = tostring(delimiter)

    -- 分隔符为空串时无法分割，直接返回 false
    if (delimiter == '') then
        return false
    end

    -- pos: 当前搜索的起始位置（1-indexed）
    -- arr: 存放分割结果的数组
    local pos, arr = 0, {}

    -- 闭包：从 pos 位置开始查找下一个分隔符
    -- 第四个参数 true 表示禁用正则匹配，进行纯文本匹配
    local find = function()
        -- 返回值为分隔符的起始索引和结束索引，如果找不到则返回 nil
        return string.find(input, delimiter, pos, true)
    end

    -- 利用泛型 for 循环依次找到每个分隔符的位置
    -- st: 分隔符的起始索引, sp: 分隔符的结束索引
    for st, sp in find do
        -- 截取 pos 到分隔符之前的部分，插入结果数组
        table.insert(arr, string.sub(input, pos, st - 1))
        -- 将搜索起点移动到分隔符之后
        pos = sp + 1
    end

    -- 将最后一段剩余部分（最后一个分隔符之后的内容）插入数组
    table.insert(arr, string.sub(input, pos))

    return arr
end
