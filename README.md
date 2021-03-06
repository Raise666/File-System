# File-System
Using C# to create a file system.

设计思想：

a. 文件系统通过磁盘上建立的一个文件来模拟一个磁盘分区上的各种文件操作，通过系统初始化文件分区获得磁盘分区的信息。 

b. 提供基本的文件系统调用：建立、读、写、打开、关闭文件和文件系统基本操作：删除文件、显示文件/目录、创建目录、删除目录、改变当前工作目录。

c. 文件系统采用树型目录结构。 

d. 用提供的文件系统的系统调用实现类似记事本的简单程序。

e.文件的逻辑结构采用流式文件，物理结构为链接结构。

f.磁盘空闲空间的管理选择位示图法，并采用显示链接分配方法。

g.目录结构为两级目录结构，采用线性搜索。

系统结构设计：

以一个文本文件FS.txt模拟硬盘，设定硬盘容量为120个block，每个block的大小为120个字节。FS.txt的长度为120*120字节。每个FCB为20字节，所以一个block至多可以存放6个FCB。

        盘块的分布：
        1#：位示图，分配与回收物理块
        2#：超级块，存放物理块的信息
        3#：存放根目录的信息
        4#-120#：数据块，用于存放文件内容
        
 功能如下：
 
 ![image](https://github.com/Raise666/File-System/blob/master/images/function.JPG)
 
 
 界面效果图：
 
 ![image](https://github.com/Raise666/File-System/blob/master/images/interface.jpg)
 
