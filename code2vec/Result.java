Similiarity ratio : 1.0
public int backPackV(int[] nums, int target) {
    if (nums == null || nums.length == 0) {
        return 0;
    }
    int n = nums.length;
    int[][] dp = new int[2][target + 1];
    // 0 items to form 0 weight
    dp[0][0] = 1;
    for (int j = 1; j <= target; j++) {
        dp[0][j] = 0;
    }
    int curr = 0, prev = 1;
    for (int i = 1; i <= n; i++) {
        prev = curr;
        curr = 1 - curr;
        for (int j = 0; j <= target; j++) {
            // Condition1:
            dp[curr][j] = dp[prev][j];
            // Condition2:
            if (j - nums[i - 1] >= 0) {
                dp[curr][j] += dp[prev][j - nums[i - 1]];
            }
        }
    }
    return dp[curr][target];
}

public int backPackV(int[] nums, int target) {
    if (nums == null || nums.length == 0) {
        return 0;
    }
    int n = nums.length;
    int[][] dp = new int[2][target + 1];
    // 0 items to form 0 weight
    dp[0][0] = 1;
    for (int j = 1; j <= target; j++) {
        dp[0][j] = 0;
    }
    int curr = 0, prev = 1;
    for (int i = 1; i <= n; i++) {
        prev = curr;
        curr = 1 - curr;
        for (int j = 0; j <= target; j++) {
            // Condition1:
            dp[curr][j] = dp[prev][j];
            // Condition2:
            if (j - nums[i - 1] >= 0) {
                dp[curr][j] += dp[prev][j - nums[i - 1]];
            }
        }
    }
    return dp[curr][target];
}

Position in method 1 (0, 24)

Position in method 2 (0, 24)

Similiarity ratio : 0.8214285714285714
public int backPackV(int[] nums, int target) {
    if (nums == null || nums.length == 0) {
        return 0;
    }
    long a = 0;
    long b = 1;
    long c = a, d = e;
    long[][] result = new int[2][target + 1];
    long m = nums.length;
    for (int j = 1; j <= target; j++) {
        result[0][j] = 0;
    }
    // 0 items to form 0 weight
    result[0][0] = 1;
    for (int i = 1; i <= m; i++) {
        b = a;
        a = 1 - a;
        for (int j = 0; j <= target; j++) {
            // Condition2:
            if (j - nums[i - 1] >= 0) {
                result[a][j] += result[b][j - nums[i - 1]];
            } else {
                result[a][j] = dp[b][j];
            }
        }
    }
    return result[a][target];
}

public int backPackV(int[] nums, int target) {
    if (nums == null || nums.length == 0) {
        return 0;
    }
    int n = nums.length;
    int[][] dp = new int[2][target + 1];
    // 0 items to form 0 weight
    dp[0][0] = 1;
    for (int j = 1; j <= target; j++) {
        dp[0][j] = 0;
    }
    int curr = 0, prev = 1;
    for (int i = 1; i <= n; i++) {
        prev = curr;
        curr = 1 - curr;
        for (int j = 0; j <= target; j++) {
            // Condition1:
            dp[curr][j] = dp[prev][j];
            // Condition2:
            if (j - nums[i - 1] >= 0) {
                dp[curr][j] += dp[prev][j - nums[i - 1]];
            }
        }
    }
    return dp[curr][target];
}

Position in method 1 (1, 3)(4, 4)(5, 5)(7, 7)(8, 8)(10, 12)(13, 13)(15, 15)(15, 15)(16, 16)(17, 17)(18, 18)(18, 18)(21, 21)(21, 23)(25, 25)(29, 29)

Position in method 2 (1, 3)(14, 14)(7, 7)(5, 5)(4, 4)(7, 9)(6, 6)(7, 7)(7, 7)(12, 12)(13, 13)(14, 14)(7, 7)(18, 18)(18, 20)(16, 16)(23, 23)
