Base method: 
public List<Role> getAllRole() throws SQLException {
    List<Role> list = new ArrayList();
    try {
        String sql = "select roleID, roleName from tblRoles";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        rs = preStm.executeQuery();
        while (rs.next()) {
            String roleID = rs.getString("roleID");
            String roleName = rs.getString("roleName");
            list.add(new Role(roleID, roleName));
        }
    } catch (Exception e) {
        e.printStackTrace();
    } finally {
        closConnection();
    }
    return list;
}
Next method
public Integer countAllUser() throws SQLException {
    int count = -1;
    try {
        String sql = "select count(*) as total from tblUsers";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        rs = preStm.executeQuery();
        if (rs.next()) {
            count = rs.getInt("total");
        }
    } catch (Exception e) {
        e.printStackTrace();
    } finally {
        closConnection();
    }
    return count;
}
Next method
public Map<String, Integer> countRole() throws SQLException {
    Map<String, Integer> map = new HashMap<>();
    try {
        String sql = "select roleID, count(roleID) as total from tblUsers group by roleID";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        rs = preStm.executeQuery();
        while (rs.next()) {
            map.put(rs.getString("roleID"), rs.getInt("total"));
        }
    } catch (Exception e) {
        e.printStackTrace();
    } finally {
        closConnection();
    }
    return map;
}
Next method
public Map<String, Integer> countRole() throws SQLException {
    Map<String, Integer> map = new HashMap<>();
    try {
        String sql = "select roleID, count(roleID) as total from tblUsers group by roleID";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        rs = preStm.executeQuery();
        while (rs.next()) {
            map.put(rs.getString("roleID"), rs.getInt("total"));
        }
    } catch (Exception e) {
        e.printStackTrace();
    } finally {
        closConnection();
    }
    return map;
}
Next method
public List<Role> getAllRole() throws SQLException {
    List<Role> list = new ArrayList();
    try {
        String sql = "select roleID, roleName from tblRoles";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        rs = preStm.executeQuery();
        while (rs.next()) {
            String roleID = rs.getString("roleID");
            String roleName = rs.getString("roleName");
            list.add(new Role(roleID, roleName));
        }
    } catch (Exception e) {
        e.printStackTrace();
    } finally {
        closConnection();
    }
    return list;
}
Next method
public List<Role> getAllRole() throws SQLException {
    List<Role> list = new ArrayList();
    try {
        String sql = "select roleID, roleName from tblRoles";
        conn = MyConnection.getMyConnection();
        preStm = conn.prepareStatement(sql);
        rs = preStm.executeQuery();
        while (rs.next()) {
            String roleID = rs.getString("roleID");
            String roleName = rs.getString("roleName");
            list.add(new Role(roleID, roleName));
        }
    } catch (Exception e) {
        e.printStackTrace();
    } finally {
        closConnection();
    }
    return list;
}
