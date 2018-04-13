using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OCR.ROZSZEZENIA;

namespace OCR.MNISTLOADER
{
    public static class MnistReader
    {
        private const string TrainImages = "emnist/emnist-letters-train-images-idx3-ubyte";
        private const string TrainLabels = "emnist/emnist-letters-train-labels-idx1-ubyte";
        private const string TestImages = "emnist/emnist-letters-test-images-idx3-ubyte";
        private const string TestLabels = "emnist/emnist-letters-test-labels-idx1-ubyte";
        
        public static IEnumerable<Image> ReadTrainingData()
        {
            foreach (var item in Read(TrainImages, TrainLabels))
            {
                yield return item;
            }
        }

        public static IEnumerable<Image> ReadTestData()
        {
            foreach (var item in Read(TestImages, TestLabels))
            {
                yield return item;
            }
        }

        private static IEnumerable<Image> Read(string imagesPath, string labelsPath)
        {
            BinaryReader labels = new BinaryReader(new FileStream(labelsPath, FileMode.Open));
            BinaryReader images = new BinaryReader(new FileStream(imagesPath, FileMode.Open));

            int magicImages = images.ReadBigInt32();
            int numberOfImages = images.ReadBigInt32();
            int width = images.ReadBigInt32();
            int height = images.ReadBigInt32();

            int magicLabel = labels.ReadBigInt32();
            int numberOfLabels = labels.ReadBigInt32();

            for (int i = 0; i < numberOfImages; i++)
            {
                var bytes = images.ReadBytes(width * height);
                var arr = new byte[height, width];

                arr.ForEach((j, k) => arr[j, k] = bytes[j * height + k]);

                /*string s = "";
                for (int x= 0; x < 28; ++x)
                {
                    for (int y = 0; y < 28; ++y)
                    {
                        if (arr[x,y] == 0)
                            s += " "; // white
                        else if (arr[x, y] == 255)
                            s += "O"; // black
                        else
                            s += "."; // gray
                     }
                            s += "\n";
                }
                Console.WriteLine(s);*/

                yield return new Image()
                {
                    Data = arr,
                    Label = labels.ReadByte()
                };
            }
        }

    }
}
    /*public static TrainingSet GenerateTrainingSet(FileInfo imagesFile, FileInfo labelsFile)
    {
        MnistImageView imageView = new MnistImageView();
        imageView.Show();

        TrainingSet trainingSet = new TrainingSet();

        List<List<double>> labels = new List<List<double>>();
        List<List<double>> images = new List<List<double>>();

        using (BinaryReader brLabels = new BinaryReader(new FileStream(labelsFile.FullName, FileMode.Open)))
        {
            using (BinaryReader brImages = new BinaryReader(new FileStream(imagesFile.FullName, FileMode.Open)))
            {
                int magic1 = brImages.ReadBigInt32(); //Reading as BigEndian
                int numImages = brImages.ReadBigInt32();
                int numRows = brImages.ReadBigInt32();
                int numCols = brImages.ReadBigInt32();

                int magic2 = brLabels.ReadBigInt32();
                int numLabels = brLabels.ReadBigInt32();

                byte[] pixels = new byte[numRows * numCols];

                // each image
                for (int imageCounter = 0; imageCounter < numImages; imageCounter++)
                {
                    List<double> imageInput = new List<double>();
                    List<double> exspectedOutput = new List<double>();

                    for (int i = 0; i < 10; i++) //generate empty exspected output
                        exspectedOutput.Add(0);

                    //read image
                    for (int p = 0; p < pixels.Length; p++)
                    {
                        byte b = brImages.ReadByte();
                        pixels[p] = b;

                        imageInput.Add(b / 255.0f); //scale in 0 to 1 range
                    }

                    //read label
                    byte lbl = brLabels.ReadByte();
                    exspectedOutput[lbl] = 1; //modify exspected output

                    labels.Add(exspectedOutput);
                    images.Add(imageInput);

                    //Debug view showing parsed image.......................
                    Bitmap image = new Bitmap(numCols, numRows);

                    for (int y = 0; y < numRows; y++)
                    {
                        for (int x = 0; x < numCols; x++)
                        {
                            image.SetPixel(x, y, Color.FromArgb(255 - pixels[x * y], 255 - pixels[x * y], 255 - pixels[x * y])); //invert colors to have 0,0,0 be white as specified by mnist
                        }
                    }

                    imageView.SetImage(image);
                    imageView.Refresh();
                    //.......................................................
                }

                brImages.Close();
                brLabels.Close();
            }
        }

        for (int i = 0; i < images.Count; i++)
        {
            trainingSet.data.Add(images[i], labels[i]);
        }

        return trainingSet;
    }*/