import os.path
import math
import time
import re
import numpy as np
from sklearn.linear_model import SGDClassifier
from sklearn.svm import SVC
from sklearn.neighbors import KNeighborsClassifier
from sklearn.tree import DecisionTreeClassifier
from sklearn.neural_network import MLPClassifier
from sklearn.model_selection import GridSearchCV
from sklearn.metrics import confusion_matrix
from sklearn.metrics import precision_recall_fscore_support

def main():
    init_time = time.time()
    
    fileName = '../sueca-logs/processedPlays.txt'
    totalSamples = 0
    abstractHands = {}
    numPlayFeatures = 0
    numHandFeatures = 0
    featuresName = []
    playClassification = ['1','2','3','4','5','6','7','8','9','10','11','12','13','14','15','16','17','18','19','20','21','22','23','24','25','26','27','28','29','30']
    numClasses = len(playClassification)
    
    if os.path.isfile(fileName):
        file = open(fileName,'r') 
        firstLine = file.readline()
        splitLine = firstLine.split(',')
        numPlayFeatures = int(splitLine[0])
        numHandFeatures = int(splitLine[1]) + 1
        numSamples = int(splitLine[2])
        X = np.ones((numSamples, numHandFeatures)) #coef of 1 to w0
        y = np.zeros(numSamples)

        secondLine = file.readline()
        splitLine = secondLine.split(',')
        for name in splitLine:
            featuresName.append(name)

        line = file.readline()
        i = 0
        while totalSamples < numSamples:
            totalSamples += 1
            line = line.replace('\n','')
            splitLine = line.split(',')
            classification = splitLine[0]
            handFeatures = splitLine[1:]
            X[i][:-1] = handFeatures
            y[i] = int(classification)
            line = file.readline()
            i += 1
        file.close()




        ### STOCHASTIC GRADIENT DESCENT
        start_time = time.time()
        print '----------------------------------------'
        print 'Linear (SGD)'
        alphas = [0.01, 0.1]
        lm_param_grid = {'alpha': alphas}
        lm_grid_search = GridSearchCV(SGDClassifier(loss='log', n_jobs=2), lm_param_grid)
        lm_grid_search.fit(X, y)
        print lm_grid_search.cv_results_['mean_test_score']
        print lm_grid_search.best_params_
        print 'time (s): ', time.time() - start_time


        ### SVM
        start_time = time.time()
        print '----------------------------------------'
        print 'SVM'
        penalties = [3.0]
        svm_param_grid = {'C': penalties}
        svm_grid_search = GridSearchCV(SVC(), svm_param_grid)
        svm_grid_search.fit(X, y)
        print svm_grid_search.cv_results_['mean_test_score']
        print svm_grid_search.best_params_
        print 'time (s): ', time.time() - start_time


        ### KNN
        start_time = time.time()
        print '----------------------------------------'
        print 'KNN'
        neighbors = [5, 10, 20]
        knn_param_grid = {'n_neighbors': neighbors}
        knn_grid_search = GridSearchCV(KNeighborsClassifier(), knn_param_grid)
        knn_grid_search.fit(X, y)
        print knn_grid_search.cv_results_['mean_test_score']
        print knn_grid_search.best_params_
        print 'time (s): ', time.time() - start_time
        

        ### DECISION TREE
        start_time = time.time()
        print '----------------------------------------'
        print 'Decision Tree'
        depths = [3, 5, 10, 15]
        dt_param_grid = {'max_depth': depths}
        dt_grid_search = GridSearchCV(DecisionTreeClassifier(), dt_param_grid)
        dt_grid_search.fit(X, y)
        print dt_grid_search.cv_results_['mean_test_score']
        print dt_grid_search.best_params_
        print 'time (s): ', time.time() - start_time


        ### NEURAL NETWORK
        start_time = time.time()
        print '----------------------------------------'
        print 'Neural Network'
        layers = [(100,50), (10)]
        nn_param_grid = {'hidden_layer_sizes': layers}
        nn_grid_search = GridSearchCV(MLPClassifier(activation='relu'), nn_param_grid)
        nn_grid_search.fit(X, y)
        print nn_grid_search.cv_results_['mean_test_score']
        print nn_grid_search.best_params_
        print 'time (s): ', time.time() - start_time


        print '________________________________________'
        print 'total time (s): ', time.time() - init_time


    else:
        print('processedPlays file not found.')
        return


if __name__ == "__main__":
    main()
