using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum Operation
{
    exit = 1, //1
    createUniSet, //2
    createCustomSet, //3
    checkElement, //4
    checkEntrance, //5
    Intersection, //6
    Union, //7
    Difference, //8
    SymmetricDifference, //9
    Complement, //10
    Print, //11
    Clear //12
}

namespace DiscreteMathLab1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool working = true;
            SetList workingList = new SetList();
            do
            {
                Console.Write('\n');

                //Проверяем корректность запроса
                bool isRequest = false;
                int request = -1;
                do
                {
                    Console.WriteLine("1 - exit\n" +
                        "2 - createUniSet\n" +
                        "3 - createCustomSet,\n" +
                        "4 - checkElement\n" +
                        "5 - checkEntrance,\n" +
                        "6 - Intersection\n" +
                        "7 - Union\n" +
                        "8 - Difference\n" +
                        "9 - SymmetricDifference\n" +
                        "10 - Complement\n" +
                        "11 - Print\n" +
                        "12 - Clear");
                    Console.Write("INPUT(from 1 to 12):");
                    //Проверка на правильный тип данных при вводе
                    isRequest = int.TryParse(Console.ReadLine(), out request);
                    //Доп.проверка, сходится ли значение запроса с имеющимися ключами к операциям
                    isRequest = request >= 1 && request <= 12 ? true : false;
                    if (!isRequest) 
                    {
                        Console.WriteLine("\nWrong type of input data or\ninput value is out of key range.\n");
                    }
                } while (!isRequest);

                Console.Write('\n');

                //Алгоритм выбирает операцию, которая соответствует запросу
                switch (request) 
                {
                    case (int)Operation.exit: 
                        {
                            Console.WriteLine("Exiting...");
                            working = false;
                            break;
                        }
                    case (int)Operation.createUniSet:
                        {
                            workingList.createUniSet();
                            break;
                        }
                    case (int)Operation.createCustomSet:
                        {
                            workingList.createCustomSet();
                            break;
                        }
                    case (int)Operation.checkElement:
                        {
                            workingList.checkElement();
                            break;
                        }
                    case (int)Operation.checkEntrance:
                        {
                            Console.WriteLine("Checking Entrance...");
                            break;
                        }
                    case (int)Operation.Intersection:
                        {
                            Console.WriteLine("Intersection in process...");
                            break;
                        }
                    case (int)Operation.Union:
                        {
                            Console.WriteLine("Union in process...");
                            break;
                        }
                    case (int)Operation.Difference:
                        {
                            Console.WriteLine("Difference in process...");
                            break;
                        }
                    case (int)Operation.SymmetricDifference:
                        {
                            Console.WriteLine("Symetric difference in progress...");
                            break;
                        }
                    case (int)Operation.Complement:
                        {
                            Console.WriteLine("Complement in progress...");
                            break;
                        }
                    case (int)Operation.Print:
                        {
                            workingList.print();
                            break;
                        }
                    case (int)Operation.Clear: 
                        {
                            workingList.clear();
                            break;
                        }
                    default: 
                        {
                            Console.WriteLine("Unknown operation");
                            break;
                        }
                }
            }
            while (working);
        }
    }
}
class SetList
{
    /*
    createUniSet, //2
    createCustomSet, //3
    checkElement, //4
    checkEntrance, //5
    Intersection, //6
    Union, //7
    Difference, //8
    SymmetricDifference, //9
    Complement //10
    Print //11
     */
    public Node head;
    public int nodeCtr;
    public List<int> uniSet;

    public SetList() 
    {
        head = null;
        nodeCtr = 0;
        uniSet = new List<int>();
    }

    private void checkInputX1X2(out int x1, out int x2) 
    {
        //Если значения приемлимы, то на выход идут x1 и x2
        bool isRangeAcceptable = false;
        do
        {
            Console.WriteLine("\nDefine the range of the uniset\n");
            Console.Write("Start border(int):");
            bool isX1Int = int.TryParse(Console.ReadLine(), out x1);
            Console.Write("End border(int):");
            bool isX2Int = int.TryParse(Console.ReadLine(), out x2);
            //Проверка на правильность типа данных
            isRangeAcceptable = isX1Int && isX2Int && x1 < x2;
            if (!isRangeAcceptable)
            {
                Console.WriteLine("\nWrong type of input data or\ninput value is out of key range.\n");
            }
        }
        while (!isRangeAcceptable);
    }
    private void checkInputRequestCreateUniset(out int request) 
    {
        /*
         Проверка ввода пользователя для создания
         универсального множества. На выход идет либо request = 1, либо request = 2
         */
        bool isRequestInt = false;
        bool isRequestAcceptable = false;
        Console.WriteLine("\nUniset already exists\n");
        //Спрашиваем о выборе операции и проверяем корректность запроса
        do
        {
            Console.WriteLine("Create new uniset (this operation deletes all created sets)?");
            Console.WriteLine("1 - yes\n2 - no");
            Console.Write("INPUT:");
            isRequestInt = int.TryParse(Console.ReadLine(), out request);
            isRequestAcceptable = isRequestInt && (request == 1 || request == 2) ? true : false;
            if (!isRequestAcceptable)
            {
                Console.WriteLine("\nWrong type of input data or\ninput value is out of key range.\n");
            }
        }
        while (!isRequestAcceptable);

    }

    private void checkInputRequestCreateCustomSet(out int request) 
    {
        bool isRequestInt = false;
        bool isRequestAcceptable = false;
        Console.WriteLine("\nCreate new set manually or randomly?\n");
        //Спрашиваем о выборе создания множества и проверяем корректность типа данных ввода
        do
        {
            Console.WriteLine("Create new set:");
            Console.WriteLine("1 - manually\n2 - randomly");
            Console.Write("INPUT:");
            isRequestInt = int.TryParse(Console.ReadLine(), out request);
            isRequestAcceptable = isRequestInt && (request == 1 || request == 2) ? true : false;
            if (!isRequestAcceptable)
            {
                Console.WriteLine("\nWrong type of input data or\ninput value is out of key range.\n");
            }
        }
        while (!isRequestAcceptable);
    }

    private void createCustomSetManualFill(out List<int> tempSetOut)
    {
        List<int> tempSet = new List<int>();
        int uniqueObject;
        bool isUniqueObjectInt = false;
        bool isUniqueObjectStop = false;
        bool isUniqueObjectAcceptable = false;

        Console.WriteLine("Write in unique objects:");
        //Заполняем множество уникальными элементами, проверяем ввод значений на корректность правильному типу данных
        do
        {
            Console.Write($"Input a unique object, type integer, range from {uniSet[0]} to {uniSet[uniSet.Count() - 1]}\nor type 'stop' to end the operation:");
            var uniqueObjectInput = Console.ReadLine();
            isUniqueObjectInt = int.TryParse(uniqueObjectInput, out uniqueObject);
            isUniqueObjectStop = uniqueObjectInput == "stop";
            isUniqueObjectAcceptable = isUniqueObjectInt && uniqueObject >= uniSet[0] && uniqueObject <= uniSet[uniSet.Count() - 1] && !tempSet.Contains(uniqueObject);
            if (isUniqueObjectStop)
            {
                Console.WriteLine("Stoping...");
            }
            else if (!isUniqueObjectAcceptable)
            {
                Console.WriteLine("\nWrong type of input data or\ninput value is out of range\n");
            }
            else
            {
                tempSet.Add(uniqueObject);
            }
        }
        while ((!(tempSet.Count() == uniSet.Count()) || !isUniqueObjectInt) && !isUniqueObjectStop);
        tempSetOut = tempSet;
    }

    private void createCustomSetRandomFill(out List<int> tempSetOut) 
    {
        List<int> tempSet = new List<int>();
        Random rnd = new Random();
        for (int i = 0; i < uniSet.Count() - rnd.Next(0, uniSet.Count() - 1); ++i)
        {
            int rndIntObject;
            do
            {
                Random rndObject = new Random();
                rndIntObject = rndObject.Next(uniSet[0], uniSet[uniSet.Count() - 1] + 1);
            }
            while (tempSet.Contains(rndIntObject));
            tempSet.Add(rndIntObject);
        }
        tempSetOut = tempSet;

    }

    private void connectNode(char key, Node nextNode, List<int> tempSet, out Node tempNodeOut)
    {
        Node tempNode = new Node(key, nextNode, tempSet);
        if (head == null)
        {
            head = tempNode;
        }
        else
        {
            Node current = head;
            for (int i = 0; i < nodeCtr - 1; ++i)
            {
                current = head.nextNode;
            }
            current.nextNode = tempNode;
        }
        tempNodeOut = tempNode;
        ++nodeCtr;
    }
    private void checkInputKey(out char key)
    {
        bool isInputChar = false;
        bool isInputCharAcceptable = false;
        bool isInputCharExists = false;

        //Просим указать ключ к множеству и проверяем корректность типа данных ввода
        do
        {
            Console.Write("Input the key-letter for the set (from A to Z):");
            isInputChar = char.TryParse(Console.ReadLine(), out key);
            isInputCharExists = false;

            //Проверка на наличие такого идентификатора в множестве
            Node current = head;
            if (nodeCtr == 0)
            {
                isInputCharExists = false;
            }
            else
            {
                for (int i = 0; i <= nodeCtr - 1; ++i)
                {
                    if (current.key == key)
                    {
                        isInputCharExists = true;
                        break;
                    }
                    current = current.nextNode;
                }
            }
            isInputCharAcceptable = isInputChar && (key > '@' || key < '[') && !isInputCharExists;

            if (!isInputCharAcceptable)
            {
                Console.WriteLine("\nWrong type of input data or\ninput value is out of range\nor the value already exists");
            }
        }
        while (!isInputCharAcceptable);
    }

    public void checkValueToCheck(out int valueToCheck) 
    {
        bool isValueInt = false;
        bool isValueAcceptable = false;

        do
        {
            Console.Write("Input the value to find (integer):");
            isValueInt = int.TryParse(Console.ReadLine(), out valueToCheck);

            isValueAcceptable = isValueInt && valueToCheck >= uniSet[0] && valueToCheck <= uniSet[uniSet.Count() - 1];

            if (!isValueAcceptable)
            {
                Console.WriteLine("\nWrong type of input data or the input value is out the range of values\n");
            }
        }
        while (!isValueAcceptable);
    }

    public void checkInputKeyToFind(out char key)
    {
        bool isKeyChar = false;
        bool isKeyInSet = false;
        bool isKeyCharAcceptable = false;
        Node current = head;

        do
        {
            Console.Write("Input the key of set to find value:");
            isKeyChar = char.TryParse(Console.ReadLine(), out key);

            if (isKeyChar)
            {
                current = head;
                for (int i = 1; i <= nodeCtr; ++i)
                {
                    if (current.key == key)
                    {
                        isKeyInSet = true;
                        break;
                    }
                    current = current.nextNode;
                }
            }
            if(isKeyChar == true && isKeyInSet == true) 
            {
                isKeyCharAcceptable = true;
            }
        }
        while (!isKeyCharAcceptable);
    }

    public void checkIsInputKeyToFindInSet(char key, int valueToCheck) 
    {
        bool isKeyInSet = false;
        Node current = head;
        for (int i = 1; i <= nodeCtr; ++i)
        {
            if (current.key == key)
            {
                for(int j = 0; j < current.set.Count()-1; ++j) 
                {
                    if (current.set[j] == valueToCheck) 
                    {
                        isKeyInSet = true;
                    }
                }
                break;
            }
            current = current.nextNode;
        }

        if (isKeyInSet) 
        {
            Console.WriteLine($"Value {valueToCheck} is in set {key}");
        }
        else 
        {
            Console.WriteLine($"Value {valueToCheck} is NOT in set {key}");
        }
            
    }

    
    //2
    public void createUniSet()
    {
        if (uniSet.Count() == 0)
        {
            int x1, x2;
            //Проверяем корректность запроса
            checkInputX1X2(out x1, out x2);

            //Создание универсального множества
            for (int i = x1; i <= x2; ++i)
            {
                uniSet.Add(i);
            }
        }
        else
        {
            int request;
            checkInputRequestCreateUniset(out request);

            if (request == 1)
            {
                this.clear();
            }
            else if (request == 2)
            {
                return;
            }
        }
    }
    //3
    public void createCustomSet()
    {
        //Проверяем, сколько множеств создал пользователь
        if (nodeCtr == 5)
        {
            Console.WriteLine("\nQuantity of sets is at maximum value\n");
        }
        else
        {
            int request;
            checkInputRequestCreateCustomSet(out request);

            //Проверяем, существует ли универсальное множество
            if (uniSet.Count() == 0)
            {
                Console.WriteLine("Uniset doesn't exists.\nCreate the uniset and try again");
            }
            else
            {
                //Множество, которое будет заполнено
                if (request == 1)
                {
                    char key;
                    checkInputKey(out key);

                    //Заполняем множество ручным вводом
                    List<int> tempSet;
                    createCustomSetManualFill(out tempSet);

                    //Передаем параметры множества в объект класса Node, связываем его с другими множествами, если такие есть, через односвяз.список
                    tempSet.Sort();
                    Node tempNode;
                    connectNode(key, null, tempSet, out tempNode);
                }
                else if (request == 2)
                {
                    char key;
                    checkInputKey(out key);

                    //Заполняем множество случайными элементами
                    List<int> tempSet = new List<int>();
                    createCustomSetRandomFill(out tempSet);

                    //Передаем параметры множества в объект класса Node, связываем его с другими множествами, если такие есть, через односвяз.список
                    tempSet.Sort();
                    Node tempNode = new Node(key, null, tempSet);
                    connectNode(key, null, tempSet, out tempNode);
                }
            }
        }
    }
    //4
    public void checkElement() 
    {
        if (nodeCtr == 0)
        {
            Console.WriteLine("\nNo sets have found\n");
        }
        else
        {

            this.print();

            //Просим указать значение для поиска и проверяем корректность типа данных ввода
            int valueToCheck;
            checkValueToCheck(out valueToCheck);

            //Просим указать значение для поиска и проверяем корректность типа данных ввода
            char key;
            checkInputKeyToFind(out key);
            
            //Проверяем есть ли в указанном множестве требуемое значение
            checkIsInputKeyToFindInSet(key, valueToCheck);

        }
    }
    //5
    public void checkEntrance() 
    {

    }
    //11
    public void print() 
    {
        if(uniSet.Count == 0) 
        {
            Console.WriteLine("Uniset is empty");
        }
        else 
        {
            Console.WriteLine($"Uniset is ");
            for (int elem = 0; elem < uniSet.Count(); ++elem)
            {
                Console.Write($"{uniSet[elem]} ");
            }
            Console.Write("\n");
        }
        if (nodeCtr == 0) 
        {
            Console.WriteLine("No sets have found");
            
        }
        else 
        {
            Console.WriteLine($"Quantity of sets: {nodeCtr}");
            Console.WriteLine("[LIST]");
            Node current = head;
            for(int i = 1;i <= nodeCtr; ++i) 
            {
                Console.Write($"{i})key: {current.key}, set: ");
                for(int elem = 0; elem < current.set.Count(); ++elem) 
                {
                    Console.Write($"{current.set[elem]} ");
                }
                Console.Write("\n");
                current = current.nextNode;
            }
        }
    }



    public void clear() 
    {
        head = null;
        nodeCtr = 0;
        uniSet.Clear();
    }
}
class Node 
{
    public char key;
    public Node nextNode;
    public List<int> set;

    public Node(char key, Node nextNode, List<int> set) 
    {
        this.key = key;
        this.nextNode = nextNode;
        this.set = set;
    }
}
