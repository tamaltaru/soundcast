# All synthesized populations are in 2000 dollars, but our other costs like parking cost and transit fare are  
# in 2010 dollars.  So we need to convert our synthetic population for 2010 dollars for consistency.

import h5py
import numpy as np
import os, sys

print 'Converting synthetic population income from 2000$ to 2010$'
INCOME_FACTOR_00_10 = 1.26

hh_file_loc = 'inputs/hh_and_persons.h5'

if not os.path.exists(hh_file_loc):
        print 'Oops you cannot convert the synthetic population income to 2010 dollars because you are missing                  the household file that should be at ' + hh_file_loc

hh_file=h5py.File(hh_file_loc, "r+")

hh_set = hh_file['Household']
hh_incomes = np.asarray(hh_set['hhincome'])
incomes_2010 = (hh_incomes * INCOME_FACTOR_00_10).astype(int)

# delete the income field so you can overwrite with new incomes
del hh_set['hhincome']
hh_file.create_dataset('Household/hhincome', data=incomes_2010, compression = 'gzip')

hh_file.close()


