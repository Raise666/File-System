# File-System
Using C# to create a file system.

以一个文本文件FS.txt模拟硬盘，设定硬盘容量为120个block，每个block的大小为120个字节。FS.txt的长度为120*120字节。每个FCB为20字节，所以一个block至多可以存放6个FCB。
        盘块的分布：
        1#：位示图，分配与回收物理块
        2#：超级块，存放物理块的信息
        3#：存放根目录的信息
        4#-120#：数据块，用于存放文件内容
