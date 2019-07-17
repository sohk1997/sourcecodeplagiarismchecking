Base method: 
public String getEncrypPassword(String password) throws NoSuchAlgorithmException {
    MessageDigest md = MessageDigest.getInstance("SHA-256");
    return new BigInteger(1, md.digest(password.getBytes())).toString(16);
}
Next method
public int maxSubArray(int[] nums, int k) {
    if (nums == null || nums.length == 0) {
        return 0;
    }
    int n = nums.length;
    int[][] dp = new int[n + 1][k + 1];
    for (int j = 1; j <= k; j++) {
        for (int i = j; i <= n; i++) {
            // ??? why i = j
            dp[i][j] = Integer.MIN_VALUE;
            int endMax = 0;
            int max = Integer.MIN_VALUE;
            for (int x = i - 1; x >= j - 1; x--) {
                // ??? why x = i-1, x >= j -1?
                endMax = Math.max(nums[x], nums[x] + endMax);
                max = Math.max(max, endMax);
                dp[i][j] = dp[i][j] < dp[x][j - 1] + max ? dp[x][j - 1] + max : dp[i][j];
            }
        }
    }
    return dp[n][k];
}
Next method
public int longestUnivaluePath(TreeNode root) {
    countUnivaluePath(root);
    return max;
}
Next method
public int searchInsert(int[] A, int target) {
    if (A == null || A.length == 0) {
        // Insert at 0 position
        return 0;
    }
    int start = 0;
    int end = A.length - 1;
    int mid = start + (end - start) / 2;
    while (start + 1 < end) {
        mid = start + (end - start) / 2;
        if (A[mid] == target) {
            return mid;
        } else if (A[mid] > target) {
            end = mid;
        } else {
            start = mid;
        }
    }
    if (A[start] >= target) {
        return start;
    } else if (A[start] < target && target <= A[end]) {
        return end;
    } else {
        return end + 1;
    }
}
Next method
public String getEncrypPassword(String password) throws NoSuchAlgorithmException {
    MessageDigest md = MessageDigest.getInstance("SHA-256");
    return new BigInteger(1, md.digest(password.getBytes())).toString(16);
}
Next method
public String getEncrypPassword(String password) throws NoSuchAlgorithmException {
    MessageDigest md = MessageDigest.getInstance("SHA-256");
    return new BigInteger(1, md.digest(password.getBytes())).toString(16);
}
