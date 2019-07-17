Base method: 
public int find(int[] array, int key) {
    // Find indexes of two corners
    int start = 0, end = (array.length - 1);
    // in array must be in range defined by corner
    while (start <= end && key >= array[start] && key <= array[end]) {
        // uniform distribution in mind.
        int pos = start + (((end - start) / (array[end] - array[start])) * (key - array[start]));
        // Condition of target found
        if (array[pos] == key)
            return pos;
        // If key is larger, key is in upper part
        if (array[pos] > key)
            end = pos - 1;
        else
            start = pos + 1;
    }
    return -1;
}
Next method
public void permute(String s, int k, int[] nums) {
    if (rst.length() != 0) {
        return;
    }
    if (s.length() == nums.length) {
        index++;
        if (index == k) {
            rst = s;
        }
        return;
    }
    for (int i = 0; i < nums.length; i++) {
        if (!s.contains(nums[i] + "")) {
            permute(s + nums[i], k, nums);
        }
    }
}
Next method
/**
 * @param matrix: a matrix of integers
 * @return: an array of integers
 */
public int[] printZMatrix(int[][] matrix) {
    int[] rst = null;
    if (matrix == null || matrix.length == 0 || matrix[0] == null || matrix[0].length == 0) {
        return rst;
    }
    int n = matrix.length;
    int m = matrix[0].length;
    rst = new int[n * m];
    if (matrix.length == 1) {
        return matrix[0];
    }
    int i = 0, j = 0;
    int ind = 0;
    rst[ind] = matrix[i][j];
    ind++;
    while (ind < rst.length) {
        // Right 1 step, or down
        if (j + 1 < m || i + 1 < n) {
            if (j + 1 < m)
                j++;
            else if (i + 1 < n)
                i++;
            rst[ind++] = matrix[i][j];
        }
        // Move left-bottom:
        while (j - 1 >= 0 && i + 1 < n) {
            rst[ind++] = matrix[++i][--j];
        }
        // Move down, or right
        if (j + 1 < m || i + 1 < n) {
            if (i + 1 < n)
                i++;
            else if (j + 1 < m)
                j++;
            rst[ind++] = matrix[i][j];
        }
        // Move right-up:
        while (j + 1 < m && i - 1 >= 0) {
            rst[ind++] = matrix[--i][++j];
        }
    }
    // end while
    return rst;
}
Next method
// Build tree based on given inorder-traversal list
public TreeNode buildBFS(ArrayList<TreeNode> list, int start, int end) {
    if (start < end) {
        int mid = start + (end - start) / 2;
        TreeNode root = list.get(mid);
        root.left = buildBFS(list, start, mid - 1);
        root.right = buildBFS(list, mid + 1, end);
        return root;
    }
    if (start == end) {
        return list.get(start);
    }
    return null;
}
Next method
public int shortestDistance(String[] words, String word1, String word2) {
    int indexWord1 = -1;
    int indexWord2 = -1;
    int distance = Integer.MAX_VALUE;
    for (int i = 0; i < words.length; i++) {
        if (words[i].equals(word1)) {
            indexWord1 = i;
        } else if (words[i].equals(word2)) {
            indexWord2 = i;
        }
        if (indexWord1 >= 0 && indexWord2 >= 0) {
            distance = Math.min(distance, Math.abs(indexWord2 - indexWord1));
        }
    }
    return distance;
}
Next method
/**
 * @param array is a sorted array
 * @param key   is a value what shoulb be found in the array
 * @return an index if the array contains the key unless -1
 */
public int find(int[] array, int key) {
    // Find indexes of two corners
    int start = 0, end = (array.length - 1);
    // in array must be in range defined by corner
    while (start <= end && key >= array[start] && key <= array[end]) {
        // uniform distribution in mind.
        int pos = start + (((end - start) / (array[end] - array[start])) * (key - array[start]));
        // Condition of target found
        if (array[pos] == key)
            return pos;
        // If key is larger, key is in upper part
        if (array[pos] < key)
            start = pos + 1;
        else
            end = pos - 1;
    }
    return -1;
}
