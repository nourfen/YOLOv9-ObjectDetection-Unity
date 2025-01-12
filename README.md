# About
This project implements the [YOLO (You Only Look Once)](https://docs.ultralytics.com/models/) detection model made by [Ultralytics](https://www.ultralytics.com/) in [Unity 6](https://unity.com/releases/unity-6). 
It uses the [YOLO IX model](https://docs.ultralytics.com/models/yolov9/)—with plans to incorporate newer versions—and leverages the [Unity Sentis package](https://unity.com/products/sentis) for ONNX model manipulation. 
Through GPU acceleration via ComputeShaders, the application enables real-time object detection from images, videos, or camera feeds with a configurable confidence and IoU (Intersection over Unon) thresholds. 
The recognized objects are limited to those included in the [COCO (Common Objects in Context)](https://cocodataset.org/#home) dataset.

# Motivation
My motivation for creating this application stems from a previous job where we considered integrating real-time object detection from a HoloLens 2 camera stream. 
Unfortunately, short deadlines prevented that feature from becoming a reality. However, my fascination with the concept led me to develop this project on my own.