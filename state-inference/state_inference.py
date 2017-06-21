from pathlib import Path
import os.path
import math
import numpy as np
from sklearn import linear_model

def main():
    processedhandsFile = Path('..\sueca-logs\processedHands.txt')
    numHands = 0
    abstractHands = {}
    
    if processedhandsFile.is_file():
        file = open(processedhandsFile,'r') 

        for line in file:
            numHands += 1
            if line in abstractHands:
                abstractHands[line] += 1
            else:
                abstractHands[line] = 1

        file.close() 
        print('abstractHands: ' + str(len(abstractHands.keys())))
        print('numHands: ' + str(numHands))


    else:
        print('ProcessedHands file not found.')
        return

    calculateFeaturesWeights(abstractHands)

def calculateFeaturesWeights(hands):
    
    n_samples = len(hands.keys())
    n_features = 5
    X = np.ones((n_samples, n_features))
    y = np.ones(n_samples)
    i = 0
    for key in hands:
        y[i] = math.log(hands[key])
        parsed = key.split('\t')
        for j in range(n_features):
            if j < n_features - 1:
                X[i][j] = float(parsed[j])
        i += 1


    clf = linear_model.SGDRegressor()
    clf.fit(X, y)
    print(clf.coef_)


if __name__ == "__main__":
    main()
